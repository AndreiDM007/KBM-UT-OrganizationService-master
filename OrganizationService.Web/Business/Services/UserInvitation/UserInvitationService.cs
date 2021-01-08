using System;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Microsoft.Extensions.Logging;
using OrganizationUserService = Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.OrganizationUserService;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation
{
    public class UserInvitationService
    {
        private readonly IUserInvitationRepository userInvitationRepository;
        private readonly Organization.OrganizationService organizationService;
        private readonly OrganizationUserService organizationUserService;
        private readonly ILogger logger;
        
        public UserInvitationService(
            IUserInvitationRepository userInvitationRepository,
            Organization.OrganizationService organizationService,
            OrganizationUserService organizationUserService,
            ILogger logger)
        {
            this.userInvitationRepository = userInvitationRepository;
            this.organizationService = organizationService;
            this.organizationUserService = organizationUserService;
            this.logger = logger;
        }

        public Result<string> CreateUserInvitation(CreateUserInvitationCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                return new Result<string>(UserInvitationServiceErrors.InvalidOrganizationIdError());
            }

            var existsOrganizationById = organizationService.ExistsOrganizationById(command.OrganizationId);
            if (existsOrganizationById.IsFailure || !existsOrganizationById.Value)
            {
                return new Result<string>(UserInvitationServiceErrors.OrganizationNotFoundError());
            }
            
            if (string.IsNullOrWhiteSpace(command.ExternalUserId))
            {
                return new Result<string>(UserInvitationServiceErrors.InvalidExternalUserIdError());
            } 
            
            if (command.OrganizationUserId <= 0)
            {
                return new Result<string>(UserInvitationServiceErrors.InvalidOrganizationUserIdError());
            }

            var existOrganizationUser = organizationUserService.ExistOrganizationUser(command.OrganizationUserId, command.OrganizationId);
            if (existOrganizationUser.IsFailure || !existOrganizationUser.Value)
            {
                return new Result<string>(UserInvitationServiceErrors.OrganizationUserNotFoundError());
            }

            var inviteGuid = Guid.NewGuid().ToString();
            var inviteId = userInvitationRepository.CreateUserInvitation(command, inviteGuid);
            logger.LogInformation($"User Invitation Created: ID {inviteId} CODE {inviteGuid}");
            
            return new Result<string>(inviteGuid);
        }

        public Result<GetUserInvitationResult> GetUserInvitation(string invitationGuid, string externalUserId)
        {
            if (string.IsNullOrWhiteSpace(invitationGuid))
            {
                return new Result<GetUserInvitationResult>(UserInvitationServiceErrors.InvalidUserInviteGuidError());
            }
            if (string.IsNullOrWhiteSpace(externalUserId))
            {
                return new Result<GetUserInvitationResult>(UserInvitationServiceErrors.InvalidExternalUserIdError());
            }

            var invitation = userInvitationRepository.GetUserInvitation(invitationGuid, externalUserId);
            if(invitation is null)
                return new Result<GetUserInvitationResult>(UserInvitationServiceErrors.UserInvitationNotFoundError());

            var organization = organizationService.GetOrganization(invitation.OrganizationId).Value;
            invitation.OrganizationName = organization.Name;

            return new Result<GetUserInvitationResult>(invitation);
        }

        public EmptyResult UpdateUserInvitation(UpdateUserInvitationCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.InvitationGuid))
            {
                return new EmptyResult(UserInvitationServiceErrors.InvalidUserInviteGuidError());
            }
            if (string.IsNullOrWhiteSpace(command.ExternalUserId))
            {
                return new EmptyResult(UserInvitationServiceErrors.InvalidExternalUserIdError());
            } 

            if (command.AcceptedAt.HasValue && command.DeclinedAt.HasValue)
            {
                return new EmptyResult(UserInvitationServiceErrors.CannotAcceptAndDeclineInvitationError());
            }

            if (!userInvitationRepository.UserInvitationExists(command.InvitationGuid, command.ExternalUserId))
            {
                return new EmptyResult(UserInvitationServiceErrors.UserInvitationNotFoundError());
            }
            
            userInvitationRepository.UpdateUserInvitation(command);
            
            return new EmptyResult();
        }

        public EmptyResult DeleteUserInvitation(string invitationGuid, string externalUserId)
        {
            if (string.IsNullOrWhiteSpace(invitationGuid))
            {
                return new EmptyResult(UserInvitationServiceErrors.InvalidUserInviteGuidError());
            }
            if (string.IsNullOrWhiteSpace(externalUserId))
            {
                return new Result<string>(UserInvitationServiceErrors.InvalidExternalUserIdError());
            } 
            if (!userInvitationRepository.UserInvitationExists(invitationGuid, externalUserId))
            {
                return new EmptyResult(UserInvitationServiceErrors.UserInvitationNotFoundError());
            }

            userInvitationRepository.DeleteUserInvitation(invitationGuid, externalUserId);
            
            return new EmptyResult();
        }
    }
}