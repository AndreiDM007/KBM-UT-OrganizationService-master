using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class OrganizationUserController : OrganizationUserService.OrganizationUserServiceBase
    {
        #region members

        private readonly Business.Services.OrganizationUser.OrganizationUserService organizationUserService;
        private readonly IMapper mapper;

        #endregion

        #region public API

        public OrganizationUserController(
            Business.Services.OrganizationUser.OrganizationUserService organizationUserService,
            OrganizationUserGrpcMapper organizationUserGrpcMapper)
        {
            this.organizationUserService = organizationUserService;
            this.mapper = organizationUserGrpcMapper.Mapper;
        }

        public override Task<QueryOrganizationUser.Types.Response> QueryOrganizationUser(
            QueryOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var queryOrganizationUserCriteria = mapper.Map<QueryOrganizationUserCriteria>(request);
            var result = organizationUserService.QueryOrganizationUser(queryOrganizationUserCriteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var response = new QueryOrganizationUser.Types.Response
            {
                Result =
                {
                    result.Value.OrganizationUsersData.Select(p => new QueryOrganizationUser.Types.OrganizationUserListView
                    {
                        ProfileValuesListView =
                        {
                            p.ProfileValues?.Select(pp => mapper.Map<QueryOrganizationUser.Types.ProfileValues>(pp))
                        },
                        OrganizationId = p.OrganizationId,
                        OrganizationUserId = p.OrganizationUserId,
                        ProfileId = p.ProfileId,
                        UserId = p.UserId
                    })
                }
            };

            return Task.FromResult(response);
        }

        public override Task<QueryOrganizationUsers.Types.Response> QueryOrganizationUsers(
            QueryOrganizationUsers.Types.Request request,
            ServerCallContext context)
        {
            var criteria = mapper.Map<QueryOrganizationUsersCriteria>(request);
            var result = organizationUserService.QueryOrganizationUsers(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var response = new QueryOrganizationUsers.Types.Response
            {
                Result =
                {
                    result.Value.Result.Select(p => new QueryOrganizationUsers.Types.OrganizationUserListView
                    {
                        OrganizationId = p.OrganizationId,
                        OrganizationUserId = p.OrganizationUserId,
                        Username = p.Username,                                              
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Email = p.Email,
                        UserType = p.UserType,
                        IsActive = p.IsActive,
                        IsPendingActivation = p.IsPendingActivation,
                        CreatedAt = p.CreatedAt,
                        LastLoginTime = p.LastLoginTime,
                        IsLocked = p.IsLocked,
                        Roles =
                        {
                            p.Roles?.Select(pp => mapper.Map<QueryOrganizationUsers.Types.RoleListView>(pp))
                        }                                                                      
                    })
                },
                Pagination = mapper.Map<QueryOrganizationUsers.Types.Pagination>(result.Value.Pagination),
            };

            return Task.FromResult(response);
        }

        public override Task<CreateOrganizationUser.Types.Response> CreateOrganizationUser(
            CreateOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var model = mapper.Map<CreateOrganizationUserCommand>(request);
            var result = this.organizationUserService.CreateOrganizationUser(model);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(
                new CreateOrganizationUser.Types.Response
                {
                    OrganizationUserId = result.Value
                });
        }

        public override Task<UpdateOrganizationUser.Types.Response> UpdateOrganizationUser(UpdateOrganizationUser.Types.Request request, ServerCallContext context)
        {
            var model = mapper.Map<UpdateOrganizationUserCommand>(request);
            var result = organizationUserService.UpdateOrganizationUser(model);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new UpdateOrganizationUser.Types.Response());
        }

        public override Task<DeleteOrganizationUser.Types.Response> DeleteOrganizationUser(
            DeleteOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var result = this.organizationUserService.DeleteOrganizationUser(request.OrganizationUserId, request.UserType, request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            

            return Task.FromResult(new DeleteOrganizationUser.Types.Response
            {
                UserId = result.Value.UserId,
                IsLastUserIdRelationship = result.Value.IsLastUserIdRelationship

            });
        }

        public override Task<GetOrganizationUser.Types.Response> GetOrganizationUser(
            GetOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.GetOrganizationUser(request.OrganizationUserId, request.UserType, request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var value = result.Value;


            var response = new GetOrganizationUser.Types.Response
            {
                OrganizationId = value.OrganizationId,
                OrganizationUserId = value.OrganizationUserId,
                ProfileId = value.ProfileId,
                UserId = value.UserId,
                Username = value.Username,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Email = value.Email,
                IsActive = value.IsActive,
                IsLocked = value.IsLocked,
                IsPendingActivation = value.IsPendingActivation,
                LastLoginAt = value.LastLoginAt,
                ProfileValuesListView =
                {
                    value.ProfileValues?.Select(pp => mapper.Map<GetOrganizationUser.Types.ProfileValue>(pp))
                },
                AssociatedOrganizationUserListView =
                {
                    value.AssociatedOrganizationUsers?.Select(pp => mapper.Map<GetOrganizationUser.Types.AssociatedOrganizationUser>(pp))
                },
                GroupListView =
                {
                    value.Groups?.Select(g => mapper.Map<GetOrganizationUser.Types.UserGroup>(g))
                },
                CreatedAt = value.CreatedAt.GetValueOrDefault()

            };
            return Task.FromResult(response);
        }

        public override Task<GetOrganizationUserByExternalUserId.Types.Response> GetOrganizationUserByExternalUserId(
            GetOrganizationUserByExternalUserId.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.GetOrganizationUserByExternalUserId(request.OrganizationId, request.UserId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }            

            var response = new GetOrganizationUserByExternalUserId.Types.Response
            {                
                OrganizationUserId = result.Value.OrganizationUserId,
                Username = result.Value.Username,
                UserType = result.Value.UserType
            };
            return Task.FromResult(response);
        }
        
        public override Task<GetOrganizationAdmin.Types.Response> GetOrganizationAdmin(
            GetOrganizationAdmin.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.GetOrganizationAdmin(request.OrganizationId, request.UserType);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var response = this.mapper.Map<GetOrganizationAdmin.Types.Response>(result.Value);
            return Task.FromResult(response);
        }

        public override Task<ExistOrganizationUser.Types.Response> ExistOrganizationUser(
            ExistOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.ExistOrganizationUser(request.OrganizationUserId, request.UserType, request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var response = new ExistOrganizationUser.Types.Response
            {
                Exists = result.Value
            };
            return Task.FromResult(response);
        }

        public override Task<ExistOrganizationUserByEmail.Types.Response> ExistOrganizationUserByEmail(ExistOrganizationUserByEmail.Types.Request request, ServerCallContext context)
        {
            var result = this.organizationUserService.ExistOrganizationUserByEmail(request.Email);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            
            return Task.FromResult(new ExistOrganizationUserByEmail.Types.Response
            {
                Exists = result.Value
            });
        }

        public override Task<ExistOrganizationUserByUsername.Types.Response> ExistOrganizationUserByUsername(
            ExistOrganizationUserByUsername.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.ExistOrganizationUserByUsername(request.Username);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new ExistOrganizationUserByUsername.Types.Response
            {
                Exists = result.Value
            });
        }

        public override Task<CreateOrganizationUsersAssociation.Types.Response> CreateOrganizationUsersAssociation(
            CreateOrganizationUsersAssociation.Types.Request request,
            ServerCallContext context)
        {
            var command = mapper.Map<CreateOrganizationUsersAssociationCommand>(request);
            var result = this.organizationUserService.CreateOrganizationUsersAssociation(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var response = new CreateOrganizationUsersAssociation.Types.Response
            {
                AssociationId = result.Value
            };
            return Task.FromResult(response);
        }

        public override Task<UpdateOrganizationUsersAssociation.Types.Response> UpdateOrganizationUsersAssociation(
            UpdateOrganizationUsersAssociation.Types.Request request,
            ServerCallContext context)
        {
            var command = mapper.Map<UpdateOrganizationUsersAssociationCommand>(request);
            var result = this.organizationUserService.UpdateOrganizationUsersAssociation(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new UpdateOrganizationUsersAssociation.Types.Response());
        }

        public override Task<DeleteOrganizationUsersAssociation.Types.Response> DeleteOrganizationUsersAssociation(DeleteOrganizationUsersAssociation.Types.Request request, ServerCallContext context)
        {
            var result = this.organizationUserService.DeleteOrganizationUsersAssociation(request.OrganizationUserId, request.AssociationType);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new DeleteOrganizationUsersAssociation.Types.Response());
        }

        public override Task<PublishCreateOrganizationUser.Types.Response> PublishCreateOrganizationUser(
            PublishCreateOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.PublishCreateOrganizationUser(request.OrganizationUserId, request.UserType, request.OrganizationId, request.TransactionId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(
                new PublishCreateOrganizationUser.Types.Response
                {
                    CreatedAt = result.Value
                });
        }

        public override Task<PublishUpdateOrganizationUser.Types.Response> PublishUpdateOrganizationUser(
            PublishUpdateOrganizationUser.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationUserService.PublishUpdateOrganizationUser(request.OrganizationUserId, request.UserType, request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(
                new PublishUpdateOrganizationUser.Types.Response
                {
                    CreatedAt = result.Value
                });
        }

        public override Task<QueryUserOrganizations.Types.Response> QueryUserOrganizations(
            QueryUserOrganizations.Types.Request request,
            ServerCallContext context)
        {
            string userId = request.UserId;
            var result = organizationUserService.QueryUserOrganizations(userId);

            return Task.FromResult(new QueryUserOrganizations.Types.Response()
            {
                OrganizationIds = { result.Value }
            });
        }

        public override Task<GetOrganizationUserType.Types.Response> GetOrganizationUserType(GetOrganizationUserType.Types.Request request, ServerCallContext context)
        {
            var result = this.organizationUserService.GetOrganizationUserType(request.OrganizationUserId, request.OrganizationId);            

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new GetOrganizationUserType.Types.Response
            {
                UserType = result.Value                
            });
        }

        public override Task<GetSingleOrganizationOrgUsers.Types.Response> GetSingleOrganizationOrgUsers(GetSingleOrganizationOrgUsers.Types.Request request, ServerCallContext context)
        {
            var result = this.organizationUserService.GetSingleOrganizationOrgUsers(request.OrganizationId);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            
            var response = new GetSingleOrganizationOrgUsers.Types.Response
            {
                Result =
                {
                    result.Value.Result.Select(m => mapper.Map<GetSingleOrganizationOrgUsers.Types.SingleOrganizationOrgUserListView>(m))
                }
            };
            return Task.FromResult(response);
        }

        public override Task<SetUserLockStatus.Types.Response> SetUserLockStatus(SetUserLockStatus.Types.Request request, ServerCallContext context)
        {
            var result = organizationUserService.SetUserLockStatus(request.ExternalUserId, request.IsLocked);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new SetUserLockStatus.Types.Response());
        }

        public override Task<SetUserPendingActivationStatus.Types.Response> SetUserPendingActivationStatus(SetUserPendingActivationStatus.Types.Request request, ServerCallContext context)
        {
            var result = organizationUserService.SetUserPendingActivationStatus(request.ExternalUserId, request.IsPendingActivation);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            
            return Task.FromResult(new SetUserPendingActivationStatus.Types.Response());
        }

        public override Task<SetLastLoginTime.Types.Response> SetLastLoginTime(SetLastLoginTime.Types.Request request, ServerCallContext context)
        {
            var result = organizationUserService.SetLastLoginTime(request.ExternalUserId, request.LastLoginAt);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new SetLastLoginTime.Types.Response());
        }

        #endregion

        #region internal API

        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<OrganizationUserAlreadyExistsError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<OrganizationUserAlreadyExistsError>().GenerateErrorDetail());
            else if (result.HasError<InvalidProfileParameterIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidProfileParameterIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationUserIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidAssociateOrgUsersDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidAssociateOrgUsersDataError>().GenerateErrorDetail());
            else if (result.HasError<AssociationAlreadyExistsError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<AssociationAlreadyExistsError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationUserIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUserTypeIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUserTypeIdError>().GenerateErrorDetail());
            else if (result.HasError<NotFoundError>())
                context.Status = new Status(StatusCode.NotFound, result.GetError<NotFoundError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationIdError>().GenerateErrorDetail());
            else if (result.HasError<EmailAlreadyInUseError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<EmailAlreadyInUseError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUserEmailError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUserEmailError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUsernameError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUsernameError>().GenerateErrorDetail());
            else if (result.HasError<AssociationDoesNotExistsWithAnotherOrganizationUser>())
                context.Status = new Status(StatusCode.NotFound, result.GetError<AssociationDoesNotExistsWithAnotherOrganizationUser>().GenerateErrorDetail());
            else if (result.HasError<InvalidExternalUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidExternalUserIdError>().GenerateErrorDetail());
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }

        #endregion
    }
}