using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class UserInvitationController : UserInvitationService.UserInvitationServiceBase
    {
        private readonly IMapper mapper;
        private readonly Business.Services.UserInvitation.UserInvitationService userInvitationService;

        public UserInvitationController(
            Business.Services.UserInvitation.UserInvitationService userInvitationService,
            UserInvitationGrpcMapper userInvitationGrpcMapper)
        {
            this.userInvitationService = userInvitationService;
            this.mapper = userInvitationGrpcMapper.Mapper;
        }
        public override Task<CreateUserInvitation.Types.Response> CreateUserInvitation(CreateUserInvitation.Types.Request request, ServerCallContext context)
        {
            var command = mapper.Map<CreateUserInvitationCommand>(request);
            var result = userInvitationService.CreateUserInvitation(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new CreateUserInvitation.Types.Response
            {
                InvitationGuid = result.Value
            });
        }

        public override Task<GetUserInvitation.Types.Response> GetUserInvitation(GetUserInvitation.Types.Request request, ServerCallContext context)
        {
            var result = userInvitationService.GetUserInvitation(request.InvitationGuid, request.ExternalUserId);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(mapper.Map<GetUserInvitation.Types.Response>(result.Value));
        }

        public override Task<UpdateUserInvitation.Types.Response> UpdateUserInvitation(UpdateUserInvitation.Types.Request request, ServerCallContext context)
        {
            var command = mapper.Map<UpdateUserInvitationCommand>(request);
            var result = userInvitationService.UpdateUserInvitation(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new UpdateUserInvitation.Types.Response());
        }

        public override Task<DeleteUserInvitation.Types.Response> DeleteUserInvitation(DeleteUserInvitation.Types.Request request, ServerCallContext context)
        {
            var result = userInvitationService.DeleteUserInvitation(request.InvitationGuid, request.ExternalUserId);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new DeleteUserInvitation.Types.Response());
        }
        
        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<InvalidOrganizationIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationIdError>().GenerateErrorDetail());
            else if (result.HasError<OrganizationNotFoundError>())
                context.Status = new Status(StatusCode.NotFound, result.GetError<OrganizationNotFoundError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationUserIdError>().GenerateErrorDetail());
            else if (result.HasError<OrganizationUserNotFoundError>())
                context.Status = new Status(StatusCode.NotFound, result.GetError<OrganizationUserNotFoundError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUserInviteGuidError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUserInviteGuidError>().GenerateErrorDetail());
            else if (result.HasError<CannotAcceptAndDeclineInvitationError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<CannotAcceptAndDeclineInvitationError>().GenerateErrorDetail());
            else if (result.HasError<UserInvitationNotFoundError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<UserInvitationNotFoundError>().GenerateErrorDetail());
            else if (result.HasError<InvalidExternalUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidExternalUserIdError>().GenerateErrorDetail());
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }
    }
}