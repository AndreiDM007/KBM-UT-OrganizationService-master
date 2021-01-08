using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.Collections;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class AuthorizationController : OrganizationService.Grpc.Generated.AuthorizationService.AuthorizationServiceBase
    {
        private readonly OrganizationService.Web.Business.Services.Authorization.AuthorizationService authorizationService;
        private readonly IMapper mapper;

        public AuthorizationController(
            OrganizationService.Web.Business.Services.Authorization.AuthorizationService authorizationService,
            AuthorizationGrpcMapper authorizationGrpcMapper)
        {
            this.authorizationService = authorizationService;
            mapper = authorizationGrpcMapper.Mapper;
        }       

        public override Task<UpdateOrganizationUserPermission.Types.Response> UpdateOrganizationUserPermission(UpdateOrganizationUserPermission.Types.Request request, ServerCallContext context)
        {
            var command = mapper.Map<UpdateOrganizationUserPermissionCommand>(request);
            var result = this.authorizationService.UpdateOrganizationUserPermission(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new UpdateOrganizationUserPermission.Types.Response());
        }       

        public override Task<QueryOrganizationUserPermission.Types.Response> QueryOrganizationUserPermission(QueryOrganizationUserPermission.Types.Request request, ServerCallContext context)
        {
            var criteria = this.mapper.Map<QueryOrganizationUserPermissionCriteria>(request);
            var result = this.authorizationService.QueryOrganizationUserPermission(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryOrganizationUserPermission.Types.Response
            {
                Pagination = this.mapper.Map<QueryOrganizationUserPermission.Types.Pagination>(result.Value.Pagination),
                Result = { this.mapper.Map<List<QueryOrganizationUserPermission.Types.OrganizationUserPermissionListView>>(result.Value.Result) }
            };
            return Task.FromResult(response);
        }
        
        public override Task<QueryOrganizationUserAndRolePermission.Types.Response> QueryOrganizationUserAndRolePermission(QueryOrganizationUserAndRolePermission.Types.Request request, ServerCallContext context)
        {
            var criteria = this.mapper.Map<QueryOrganizationUserAndRolePermissionCriteria>(request);
            var result = this.authorizationService.QueryOrganizationUserAndRolePermission(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryOrganizationUserAndRolePermission.Types.Response
            {
                OrganizationUserId = result.Value.OrganizationUserId,
                OrganizationId = result.Value.OrganizationId,
                UserId = result.Value.UserId,
                Permissions = { result.Value.Permissions } 
            };
            return Task.FromResult(response);
        }       

        public override Task<CreateOrganizationUserRole.Types.Response> CreateOrganizationUserRole(CreateOrganizationUserRole.Types.Request request, ServerCallContext context)
        {
            var model = this.mapper.Map<CreateOrganizationUserRoleCommand>(request);
            var result = this.authorizationService.CreateOrganizationUserRole(model);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new CreateOrganizationUserRole.Types.Response
            {
                OrganizationUserRoleId = result.Value
            });
        }

        public override Task<DeleteOrganizationUserRole.Types.Response> DeleteOrganizationUserRole(DeleteOrganizationUserRole.Types.Request request, ServerCallContext context)
        {
            var model = this.mapper.Map<DeleteOrganizationUserRoleCommand>(request);
            var result = this.authorizationService.DeleteOrganizationUserRole(model);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new DeleteOrganizationUserRole.Types.Response());
        }

        public override Task<QueryOrganizationUserRole.Types.Response> QueryOrganizationUserRole(QueryOrganizationUserRole.Types.Request request, ServerCallContext context)
        {
            var criteria = this.mapper.Map<QueryOrganizationUserRoleCriteria>(request);
            var result = this.authorizationService.QueryOrganizationUserRole(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryOrganizationUserRole.Types.Response
            {
                Pagination = this.mapper.Map<QueryOrganizationUserRole.Types.Pagination>(result.Value.Pagination),
                Result = { this.mapper.Map<List<QueryOrganizationUserRole.Types.OrganizationUserRoleListView>>(result.Value.Result) }
            };
            return Task.FromResult(response);
        }

        public override Task<ExistRolePermissionById.Types.Response> ExistRolePermissionById(ExistRolePermissionById.Types.Request request, ServerCallContext context)
        {
            var result = authorizationService.ExistRolePermissionById(request.RoleId, request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new ExistRolePermissionById.Types.Response
            {
                Exists = result.Value
            });
        }
        
        public override Task<CreateRolePermission.Types.Response> CreateRolePermission(CreateRolePermission.Types.Request request, ServerCallContext context)
        {
            var model = this.mapper.Map<CreateRolePermissionCommand>(request);
            var result = this.authorizationService.CreateRolePermission(model);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new CreateRolePermission.Types.Response
            {
                RolePermissionId = result.Value
            });
        }

        public override Task<UpdateRolePermission.Types.Response> UpdateRolePermission(UpdateRolePermission.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<UpdateRolePermissionCommand>(request);
            var result = this.authorizationService.UpdateRolePermission(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new UpdateRolePermission.Types.Response());
        }

        public override Task<DeleteRolePermission.Types.Response> DeleteRolePermission(DeleteRolePermission.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<DeleteRolePermissionCommand>(request);
            var result = this.authorizationService.DeleteRolePermission(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new DeleteRolePermission.Types.Response());
        }

        public override Task<QueryRolePermission.Types.Response> QueryRolePermission(QueryRolePermission.Types.Request request, ServerCallContext context)
        {
            var criteria = this.mapper.Map<QueryRolePermissionCriteria>(request);
            var result = this.authorizationService.QueryRolePermission(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryRolePermission.Types.Response
            {
                Result = { this.mapper.Map<List<QueryRolePermission.Types.RolePermissionListView>>(result.Value.Result)},
                Pagination = this.mapper.Map<QueryRolePermission.Types.Pagination>(result.Value.Pagination)

            };
            return Task.FromResult(response);
        }

        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<EntityAlreadyExistsError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<EntityAlreadyExistsError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidCreateOrganizationDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidCreateOrganizationDataError>().GenerateErrorDetail());
            else if (result.HasError<InvalidExternalUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidExternalUserIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUpdateOrganizationNameError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUpdateOrganizationNameError>().GenerateErrorDetail());
            else if (result.HasError<InvalidDeleteEntityError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidDeleteEntityError>().GenerateErrorDetail());
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }
    }
}