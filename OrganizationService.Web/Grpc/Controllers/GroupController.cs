using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;
using Microsoft.Extensions.Logging;
using GroupServiceBase = Kebormed.Core.OrganizationService.Grpc.Generated.GroupService.GroupServiceBase;
using GroupService = Kebormed.Core.OrganizationService.Web.Business.Services.Group.GroupService;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class GroupController : GroupServiceBase
    {
        private readonly GroupService service;
        private readonly IMapper mapper;
        private readonly ILogger<GroupController> logger;

        public GroupController(GroupService service, GroupGrpcMapper mapper, ILogger<GroupController> logger)
        {
            this.service = service;
            this.mapper = mapper.Mapper;
            this.logger = logger;
        }

        public override Task<CreateGroup.Types.Response> CreateGroup(CreateGroup.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<CreateGroupCommand>(request);
            var result = this.service.CreateGroup(command);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new CreateGroup.Types.Response
            {
                GroupId = result.Value
            });
        }

        public override Task<UpdateGroup.Types.Response> UpdateGroup(UpdateGroup.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<UpdateGroupCommand>(request);

            var result = this.service.UpdateGroup(command);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new UpdateGroup.Types.Response());
        }

        public override Task<QueryGroupMember.Types.Response> QueryGroupMember(QueryGroupMember.Types.Request request, ServerCallContext context)
        {
            var criteria = mapper.Map<QueryGroupMemberCriteria>(request);            
            var result = service.QueryGroupMember(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryGroupMember.Types.Response
            {
                Pagination = mapper.Map<QueryGroupMember.Types.Pagination>(result.Value.Pagination),
                Result =
                {
                    result.Value.Result.Select(lm => mapper.Map<QueryGroupMember.Types.GroupMemberListView>(lm))
                }
            };
            return Task.FromResult(response);
        }
        
        public override Task<QueryMemberGroup.Types.Response> QueryMemberGroup(QueryMemberGroup.Types.Request request, ServerCallContext context)
        {
            var criteria = mapper.Map<QueryMemberGroupCriteria>(request);            
            var result = service.QueryMemberGroup(criteria);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryMemberGroup.Types.Response
            {                
                Result =
                {
                    result.Value.Result.Select(lm => mapper.Map<QueryMemberGroup.Types.MemberGroupListView>(lm))
                }
            };
            return Task.FromResult(response);
        }

        public override Task<DeleteGroup.Types.Response> DeleteGroup(DeleteGroup.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<DeleteGroupCommand>(request);

            var result = this.service.DeleteGroup(command);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new DeleteGroup.Types.Response());
        }

        public override Task<AssociateOrganizationUserToGroup.Types.Response> AssociateOrganizationUserToGroup(AssociateOrganizationUserToGroup.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<AssociateOrganizationUserToGroupCommand>(request);
            command.AllowedUserTypes = request.AllowedUserTypes.Select(type => type.Value).ToList();

            var result = this.service.AssociateOrganizationUserToGroup(command);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new AssociateOrganizationUserToGroup.Types.Response());
        }

        public override Task<DisassociateOrganizationUserFromGroup.Types.Response> DisassociateOrganizationUserFromGroup(DisassociateOrganizationUserFromGroup.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<DisassociateOrganizationUserFromGroupCommand>(request);

            var result = this.service.DisassociateOrganizationUserFromGroup(command);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new DisassociateOrganizationUserFromGroup.Types.Response());
        }

        public override Task<QueryGroups.Types.Response> QueryGroups(QueryGroups.Types.Request request, ServerCallContext context)
        {            
            var result = service.QueryGroups(request.OrganizationId);
            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryGroups.Types.Response
            {                
                Result =
                {
                    result.Value.Result.Select(lm => mapper.Map<QueryGroups.Types.GroupListView>(lm))
                }
            };
            return Task.FromResult(response);
        }

        public override Task<ExistGroupByName.Types.Response> ExistGroupByName(ExistGroupByName.Types.Request request, ServerCallContext context)
        {
            var result = service.ExistGroupByName(request.Name, request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new ExistGroupByName.Types.Response
            {
                Exists = result.Value
            });
        }

        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<GroupAlreadyExistsError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<GroupAlreadyExistsError>().GenerateErrorDetail());
            else if (result.HasError<InvalidCreateGroupDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidCreateGroupDataError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUpdateGroupDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUpdateGroupDataError>().GenerateErrorDetail());
            else if (result.HasError<InvalidDeleteGroupDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidDeleteGroupDataError>().GenerateErrorDetail());
            else if (result.HasError<InvalidQueryGroupMemberDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidQueryGroupMemberDataError>().GenerateErrorDetail());           
            else if (result.HasError<InvalidAssociateOrgUsersDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidAssociateOrgUsersDataError>().GenerateErrorDetail());
            else if (result.HasError<GroupNotFoundError>())
                context.Status = new Status(StatusCode.NotFound, result.GetError<GroupNotFoundError>().GenerateErrorDetail());
            else if (result.HasError<AssociationAlreadyExistsError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<AssociationAlreadyExistsError>().GenerateErrorDetail());
            else if (result.HasError<OrganizationUserNotFoundError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<OrganizationUserNotFoundError>().GenerateErrorDetail());            
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }
    }
}
