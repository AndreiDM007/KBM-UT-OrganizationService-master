using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class ProfileController : ProfileService.ProfileServiceBase
    {
        private readonly Business.Services.Profile.ProfileService profileService;
        private readonly IMapper mapper;

        public ProfileController(Business.Services.Profile.ProfileService profileService,
            ProfileGrpcMapper profileGrpcMapper)
        {
            this.profileService = profileService;
            mapper = profileGrpcMapper.Mapper;
        }

        public override Task<CreateProfile.Types.Response> CreateProfile(CreateProfile.Types.Request request, ServerCallContext context)
        {
            var model = new CreateProfileCommand
            {
                TransactionId = request.TransactionId,
                OrganizationUserId = request.OrganizationUserId,
                OrganizationId =   request.OrganizationId,
                ProfileValues = request.ProfileValuesListView?.Select(pv => new ProfileValueCreateModel
                {
                    Value = pv.Value,
                    ProfileParameterId = pv.ProfileParameterId.Value,
                    TransactionId = pv.TransactionId
                }).ToList()
            };
            var result = this.profileService.CreateProfile(model);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new CreateProfile.Types.Response
            {
                ProfileId = result.Value
            });
        }

        public override Task<GetProfile.Types.Response> GetProfile(GetProfile.Types.Request request, ServerCallContext context)
        {
            var result = this.profileService.GetProfile(request.ProfileId, request.OrganizationId);


            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var getProfileResult = result.Value;

            var response = new GetProfile.Types.Response
            {
                ProfileId = getProfileResult.ProfileId,
                OrganizationUserId = getProfileResult.OrganizationUserId,
                ProfileValuesListView =
                {
                    getProfileResult.ProfileValues?.Select(pp => mapper.Map<ProfileValue>(pp))
                }
            };
            return Task.FromResult(response);
        }

        public override Task<UpdateProfile.Types.Response> UpdateProfile(UpdateProfile.Types.Request request, ServerCallContext context)
        {
            var model = new UpdateProfileCommand
            {
                OrganizationUserId = request.OrganizationUserId.Value,
                OrganizationId = request.OrganizationId.Value,
                ProfileId = request.ProfileId.Value,
                ProfileValues = request.ProfileValuesListView?.Select(pv => new ProfileValueUpdateModel
                {
                    Value = pv.Value,
                    ProfileParameterId = pv.ProfileParameterId.Value
                }).ToList()
            };
            var result = this.profileService.UpdateProfile(model);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new UpdateProfile.Types.Response());
        }

        public override Task<DeleteProfile.Types.Response> DeleteProfile(DeleteProfile.Types.Request request, ServerCallContext context)
        {
            var model =  new DeleteProfileCommand
            {
                OrganizationUserId = request.OrganizationUserId.GetValueOrDefault(),
                ProfileId = request.ProfileId.GetValueOrDefault() 
            };
            var result = this.profileService.DeleteProfile(model);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new DeleteProfile.Types.Response { });
        }

        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<InvalidOrganizationUserIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationUserIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidProfileParameterIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidProfileParameterIdError>().GenerateErrorDetail());
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }
    }
}