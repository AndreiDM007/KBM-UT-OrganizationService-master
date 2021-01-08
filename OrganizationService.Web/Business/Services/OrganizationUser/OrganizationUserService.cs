using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Kebormed.Core.Business.Results;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser
{
    public class OrganizationUserService
    {
        #region members

        private readonly IOrganizationUserRepository organizationUserRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IOrganizationUserRoleRepository roleRepository;
        private readonly ILogger<OrganizationUserService> logger;
        private readonly IMessagePublisher publisher;

        #endregion

        #region public API

        public OrganizationUserService(
            IOrganizationUserRepository organizationUserRepository,
            IOrganizationRepository organizationRepository,
            IGroupRepository groupRepository,
            IProfileRepository profileRepository,
            IOrganizationUserRoleRepository roleRepository,
            IMessagePublisher publisher,
            ILogger<OrganizationUserService> logger)
        {
            this.organizationUserRepository = organizationUserRepository;
            this.organizationRepository = organizationRepository;
            this.groupRepository = groupRepository;
            this.profileRepository = profileRepository;
            this.roleRepository = roleRepository;
            this.logger = logger;
            this.publisher = publisher;
        }

        public Result<GetOrganizationUserResult> GetOrganizationUser(int organizationUserId, int userType, int organizationId)
        {
            var organizationUserResult = organizationUserRepository.GetOrganizationUser(organizationUserId, userType, organizationId);

            if (organizationUserResult is null)
            {
                return new Result<GetOrganizationUserResult>(new NotFoundError());
            }

            return new Result<GetOrganizationUserResult>(organizationUserResult);
        }

        public Result<GetOrganizationUserByExternalUserIdResult> GetOrganizationUserByExternalUserId(int organizationId, string userId)
        {
            var result = organizationUserRepository.GetOrganizationUserByExternalUserId(organizationId, userId);

            if (result is null)
            {
                return new Result<GetOrganizationUserByExternalUserIdResult>(new NotFoundError());
            }

            return new Result<GetOrganizationUserByExternalUserIdResult>(result);
        }
        
        public Result<GetOrganizationAdminResult> GetOrganizationAdmin(int organizationId, int userType)
        {
            var organizationAdminResult = organizationUserRepository.GetOrganizationAdmin(organizationId, userType);
            return new Result<GetOrganizationAdminResult>(organizationAdminResult);
        }

        public Result<int> CreateOrganizationUser(CreateOrganizationUserCommand command)
        {
            this.logger.LogInformation("{}, {}, {}", command.UserType, command.OrganizationId, command.UserId);

            if (organizationUserRepository.OrganizationUserExists(command.UserId, command.UserType, command.OrganizationId))
            {
                return new Result<int>(OrganizationUserServiceErrors.OrganizationUserAlreadyExists());
            }

            var organizationUserId = organizationUserRepository.CreateOrganizationUser(command);
            return new Result<int>(organizationUserId);
        }

        public EmptyResult UpdateOrganizationUser(UpdateOrganizationUserCommand command)
        {
            this.logger.LogInformation("{} {}", command.OrganizationUserId, command.UserType);

            var organizationUser =
                organizationUserRepository.GetOrganizationUser(command.OrganizationUserId, command.UserType, command.OrganizationId);
            if (organizationUser == null)
            {
                return new EmptyResult(OrganizationUserServiceErrors.NotFoundError());
            }

            if (!string.IsNullOrWhiteSpace(command.Email) &&
               !string.Equals(command.Email, organizationUser.Email, StringComparison.CurrentCultureIgnoreCase) &&
               organizationUserRepository.IsEmailInUse(command.Email, organizationUser.OrganizationUserId))
            {
                return new EmptyResult(OrganizationUserServiceErrors.EmailAlreadyInUseError());
            }

            organizationUserRepository.UpdateOrganizationUser(command);
            return new EmptyResult();
        }

        public Result<AnyOrganizationUserWithSharedUserId> DeleteOrganizationUser(int? organizationUserId, int? userType, int? organizationId)
        {
            if (!organizationId.HasValue)
            {
                return new Result<AnyOrganizationUserWithSharedUserId>(OrganizationUserServiceErrors.InvalidOrganizationId());
            }
            if (!userType.HasValue)
            {
                return new Result<AnyOrganizationUserWithSharedUserId>(OrganizationUserServiceErrors.InvalidUserTypeId());
            }
            if (!organizationUserId.HasValue)
            {
                return new Result<AnyOrganizationUserWithSharedUserId>(OrganizationUserServiceErrors.InvalidOrganizationUserId());
            }
            if (!organizationUserRepository.OrganizationUserExists(organizationUserId.Value, organizationId.Value))
            {
                return new Result<AnyOrganizationUserWithSharedUserId>(OrganizationUserServiceErrors.NotFoundError());
            }
            
            var deletedAt = organizationUserRepository.DeleteOrganizationUser(organizationUserId.Value, userType.Value, organizationId.Value);

            groupRepository.DisassociateOrganizationUserFromAllGroups(new DisassociateOrganizationUserFromAllGroupsCommand
            {
                OrganizationId = organizationId.Value,
                OrganizationUserId = organizationUserId.Value
            });

            var isLastUserIdRelationship = organizationUserRepository.AnyOrganizationUserWithSharedUserId(organizationUserId.Value);
            isLastUserIdRelationship.DeletedAt = deletedAt;
            
            var publishResult = PublishDeleteOrganizationUser(
                organizationUserId.Value,
                userType.Value,
                deletedAt,
                organizationId.Value);

            return new Result<AnyOrganizationUserWithSharedUserId>(isLastUserIdRelationship);
        }

        public Result<long> PublishCreateOrganizationUser(int organizationUserId, int userType, int organizationId, string transactionId)
        {
            // need to make sure the org user actually exists
            var organizationUserData = organizationUserRepository.GetOrganizationUser(organizationUserId, userType, organizationId);
            if (organizationUserData == null)
            {
                return new Result<long>(OrganizationUserServiceErrors.InvalidOrganizationUserId());
            }

            // need to get a hold of the OrgUser metadata
            var profileData = profileRepository.GetProfile(organizationUserData.ProfileId, organizationId);
            var organizationUserRoles = roleRepository.GetOrganizationUserRoles(organizationUserId, organizationId);

            var roles = string.Empty;
            if (organizationUserRoles.Roles.Count > 0)
                roles = JsonConvert.SerializeObject(organizationUserRoles.Roles);
                      
            // need to send the domain event to potential interested subscribers 
            var message = publisher.CreateMessage(new OrganizationUserAddedDomainEvent
            {
                TransactionId = transactionId,
                UserType = userType,
                OrganizationUserId = organizationUserId,
                OrganizationId = organizationUserData.OrganizationId,
                UserId = organizationUserData.UserId,
                Username = organizationUserData.Username,
                FirstName = organizationUserData.FirstName,
                LastName = organizationUserData.LastName,
                Email = organizationUserData.Email,
                IsActive = organizationUserData.IsActive,
                CreatedAt = organizationUserData.CreatedAt,
                IsLocked = organizationUserData.IsLocked,
                IsPendingActivation = organizationUserData.IsPendingActivation,
                Profile = new Dictionary<int, string>(profileData.ProfileValues.Select(p => new KeyValuePair<int, string>(p.ProfileParameterId, p.Value))),
                Roles = roles
                // RelatedOrganizationUsers = TBD                 
            });
            publisher.Publish(message);

            return new Result<long>(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public Result<long> PublishUpdateOrganizationUser(int organizationUserId, int userType, int organizationId)
        {

            // need to make sure the org user actually exists
            var organizationUserData = organizationUserRepository.GetOrganizationUser(organizationUserId, userType, organizationId);
            if (organizationUserData == null)
            {
                return new Result<long>(OrganizationUserServiceErrors.InvalidOrganizationUserId());
            }

            // need to get a hold of the OrgUser metadata
            var profileData = profileRepository.GetProfile(organizationUserData.ProfileId, organizationId);
            var organizationUserRoles = roleRepository.GetOrganizationUserRoles(organizationUserId, organizationId);

            var roles = string.Empty;
            if (organizationUserRoles.Roles.Count > 0)
                roles = JsonConvert.SerializeObject(organizationUserRoles.Roles);

            // need to send the domain event to potential interested subscribers 
            var message = publisher.CreateMessage(new OrganizationUserUpdatedDomainEvent
            {
                UserType = userType,
                OrganizationUserId = organizationUserId,
                OrganizationId = organizationUserData.OrganizationId,
                UserId = organizationUserData.UserId,
                Username = organizationUserData.Username,
                FirstName = organizationUserData.FirstName,
                LastName = organizationUserData.LastName,
                Email = organizationUserData.Email,
                IsActive = organizationUserData.IsActive,
                CreatedAt = organizationUserData.CreatedAt,
                Profile = new Dictionary<int, string>(profileData.ProfileValues.Select(p => new KeyValuePair<int, string>(p.ProfileParameterId, p.Value))),
                Roles = roles,
                IsLocked = organizationUserData.IsLocked,
                IsPendingActivation = organizationUserData.IsPendingActivation
                // RelatedOrganizationUsers = TBD                 
            });
            publisher.Publish(message);

            return new Result<long>(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public Result<long> PublishDeleteOrganizationUser(int? organizationUserId, int? userType, long valueDeletedAt, int organizationId)
        {
            if (!organizationUserId.HasValue)
            {
                return new Result<long>(OrganizationUserServiceErrors.InvalidOrganizationUserId());
            }

            if (!userType.HasValue)
            {
                return new Result<long>(OrganizationUserServiceErrors.InvalidUserTypeId());
            }

            // need to send the domain event to potential interested subscribers 
            var message = publisher.CreateMessage(new OrganizationUserDeletedDomainEvent
            {
                UserType = userType.Value,
                OrganizationUserId = organizationUserId.Value,
                OrganizationId = organizationId,
                DeletedAt = valueDeletedAt
            });

            publisher.Publish(message);

            return new Result<long>(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        public Result<QueryOrganizationUserResult> QueryOrganizationUser(QueryOrganizationUserCriteria criteria)
        {
            var queryOrganizationUsersResult = this.organizationUserRepository.QueryOrganizationUser(criteria);
            if (queryOrganizationUsersResult is null)
            {
                return new Result<QueryOrganizationUserResult>(new InvalidOrganizationUserIdError());
            }

            return new Result<QueryOrganizationUserResult>(queryOrganizationUsersResult);
        }

        public Result<QueryOrganizationUsersResult> QueryOrganizationUsers(QueryOrganizationUsersCriteria criteria)
        {
            if (criteria == null)
            {
                return new Result<QueryOrganizationUsersResult>(OrganizationUserServiceErrors.InvalidQueryParameters(nameof(criteria)));
            }

            if (criteria.Page == null || criteria.Page.Value <= 0)
            {
                criteria.Page = 1;
            }

            if (criteria.PageSize == null || criteria.PageSize.Value <= 0)
            {
                criteria.PageSize = 10;
            }

            string[] sortableFields = { "firstname", "lastname", "email", "username", "createdat", "islocked", "ispendingactivation", "isactive", "lastlogintime" };

            if (!sortableFields.Contains(criteria.OrderBy, StringComparer.OrdinalIgnoreCase))
            {
                criteria.OrderBy = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(criteria.Direction)) criteria.Direction = "ASC";
            if (!criteria.Direction.ToUpper().Equals("ASC") && !criteria.Direction.ToUpper().Equals("DESC"))
            {
                criteria.Direction = "ASC";
            }

            var result = organizationUserRepository.QueryOrganizationUsers(criteria);

            return new Result<QueryOrganizationUsersResult>(result);            
        }

        /// <summary>
        /// Associates two organization users with a given association type.
        /// </summary>
        /// <param name="associationCommand"></param>
        /// <returns></returns>
        public Result<int> CreateOrganizationUsersAssociation(CreateOrganizationUsersAssociationCommand associationCommand)
        {
            if (associationCommand == null)
            {
                var error = OrganizationUserServiceErrors.InvalidAssociateOrgUsersData();
                logger.LogError(error.Code);
                return new Result<int>(error);
            }

            if (!this.organizationUserRepository.OrganizationUserExists(associationCommand.OrganizationUserId1, associationCommand.OrganizationId) ||
                !this.organizationUserRepository.OrganizationUserExists(associationCommand.OrganizationUserId2, associationCommand.OrganizationId)
            )
            {
                var error = OrganizationUserServiceErrors.InvalidOrganizationUserId();
                logger.LogError(error.Code);
                return new Result<int>(error);
            }

            var associationExists = this.organizationUserRepository.AssociationExists(associationCommand);
            if (associationExists)
            {
                var error = OrganizationUserServiceErrors.AssociationAlreadyExists();
                logger.LogError(error.Code);
                return new Result<int>(error);
            }

            //Checks if the primary organizationUser already has an association of the same type with a different user.
            var associationTypeExists = this.organizationUserRepository.AssociationTypeExists(associationCommand.OrganizationUserId1, associationCommand.AssociationType);
            if (associationTypeExists)
            {
                var error = OrganizationUserServiceErrors.AssociationOfTypeAlreadyExistsWithAnotherUser();
                logger.LogError(error.Code);
                return new Result<int>(error);
            }


            var result = this.organizationUserRepository.CreateAssociationOrganizationUser(associationCommand);
            return new Result<int>(result);
        }

        public EmptyResult DeleteOrganizationUsersAssociation(int organizationUserId, int associationType)
        {
            var associationTypeExists = this.organizationUserRepository.AssociationTypeExists(organizationUserId, associationType);
            if (!associationTypeExists)
            {
                var error = OrganizationUserServiceErrors.AssociationDoesNotExistsWithAnotherOrganizationUser();
                return new EmptyResult(error);
            }

            this.organizationUserRepository.DeleteAssociationOrganizationUser(organizationUserId, associationType);
            return new EmptyResult();
        }

        /// <summary>
        /// To keep historical data an Update will soft delete a record and create a new one
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public EmptyResult UpdateOrganizationUsersAssociation(UpdateOrganizationUsersAssociationCommand command)
        {
            //if we are updating an already existing association, there is nothing to change
            var associationExists = this.organizationUserRepository.AssociationExists(command);
            if (!associationExists)
            {
                DeleteOrganizationUsersAssociation(command.OrganizationUserId1, command.AssociationType);
                CreateOrganizationUsersAssociation(command);
            }
            return new EmptyResult();
        }

        public Result<List<int>> QueryUserOrganizations(string userId)
        {
            var organizationIds = this.organizationUserRepository.QueryUserOrganizations(userId);

            if (organizationIds is null)
            {
                return new Result<List<int>>(new InvalidOrganizationUserIdError());
            }

            return new Result<List<int>>(organizationIds);
        }

        public Result<bool> ExistOrganizationUser(int? organizationUserId, int? userType, int? organizationId)
        {
            if (!organizationUserId.HasValue)
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidOrganizationUserId());
            }
            if (!userType.HasValue)
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidUserTypeIdError());
            }
            if (!organizationId.HasValue)
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidOrganizationId());
            }

            var organizationUserResult = organizationUserRepository.OrganizationUserExists(organizationUserId.Value, userType.Value, organizationId.Value);

            return new Result<bool>(organizationUserResult);
        }
        
        public Result<bool> ExistOrganizationUser(int organizationUserId, int organizationId)
        {
            if (organizationUserId <= 0)
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidOrganizationUserId());
            }
            if (organizationId <= 0)
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidOrganizationId());
            }

            var organizationUserResult = organizationUserRepository.OrganizationUserExists(organizationUserId, organizationId);

            return new Result<bool>(organizationUserResult);
        }

        public Result<bool> ExistOrganizationUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new Result<bool>(new InvalidUserEmailError());
            }    

            var result =
                organizationUserRepository.OrganizationUserExistsByEmail(email);
            return new Result<bool>(result);
        }

        public Result<bool> ExistOrganizationUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new Result<bool>(new InvalidUsernameError());
            }

            var result =
                organizationUserRepository.OrganizationUserExistsByUsername(username);
            return new Result<bool>(result);
        }

        public Result<int> GetOrganizationUserType(int organizationUserId, int organizationId)
        {
            if (!organizationUserRepository.OrganizationUserExists(organizationUserId, organizationId))
            {
                return new Result<int>(new NotFoundError());
            }

            var result = organizationUserRepository.GetOrganizationUserType(organizationUserId, organizationId);
            return new Result<int>(result.GetValueOrDefault());
        }

        public Result<GetSingleOrganizationOrgUsersResult> GetSingleOrganizationOrgUsers(int organizationId)
        {
            if (organizationId <= 0 || !organizationRepository.OrganizationExists(organizationId))
            {
                return new Result<GetSingleOrganizationOrgUsersResult>(OrganizationUserServiceErrors.NotFoundError());
            }
            var result = organizationUserRepository.GetSingleOrganizationOrgUsers(organizationId);
            return new Result<GetSingleOrganizationOrgUsersResult>(result);
        }

        public EmptyResult SetUserLockStatus(string externalUserId, bool isLocked)
        {
            if (string.IsNullOrWhiteSpace(externalUserId))
            {
                return new EmptyResult(OrganizationUserServiceErrors.InvalidExternalUserIdError());
            }
            
            //update org user
            var userResult = organizationUserRepository.SetOrganizationUserLockedStatus(externalUserId, isLocked);
            
            //publish changes
            var setStatusMsg = publisher.CreateMessage(new OrganizationUserSetLockedStatusDomainEvent
            {
                ExternalUserId = externalUserId,
                UserType = userResult.UserType,
                IsLocked = isLocked
            });

            publisher.Publish(setStatusMsg);

            return new EmptyResult();
        }
        
        public EmptyResult SetUserPendingActivationStatus(string externalUserId, bool isPendingActivation)
        {
            if (string.IsNullOrWhiteSpace(externalUserId))
            {
                return new EmptyResult(OrganizationUserServiceErrors.InvalidExternalUserIdError());
            }
            
            //update org user
            var userResult = organizationUserRepository.SetUserPendingActivationStatus(externalUserId, isPendingActivation);

            var message = publisher.CreateMessage(new OrganizationUserSetPendingStatusDomainEvent
            {
                ExternalUserId = externalUserId,
                UserType = userResult.UserType,
                IsPendingActivation = isPendingActivation
            });
            publisher.Publish(message);
            return new EmptyResult();
        }
        
        public EmptyResult SetLastLoginTime(string externalUserId, long lastLoginTimestamp)
        {
            if (string.IsNullOrWhiteSpace(externalUserId))
            {
                return new EmptyResult(OrganizationUserServiceErrors.InvalidExternalUserIdError());
            }
            
            //update org user
            var userResult = organizationUserRepository.SetLastLoginTime(externalUserId, lastLoginTimestamp);
            
            //publish changes
            var setStatusMsg = publisher.CreateMessage(new OrganizationUserSetLastLoginDomainEvent
            {
                ExternalUserId = externalUserId,
                UserType = userResult.UserType,
                LastLoginAt = lastLoginTimestamp
            });

            publisher.Publish(setStatusMsg);

            return new EmptyResult();
        }

        #endregion
    }
}