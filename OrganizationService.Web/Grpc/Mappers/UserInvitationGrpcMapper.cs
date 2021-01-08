using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Kebormed.Core.Communication.Grpc.Extensions;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Mappers
{
    public class UserInvitationGrpcMapper
    {
        public IMapper Mapper { get; }

        public UserInvitationGrpcMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Int64Value, long?>().ConvertUsing(i => i.GetValue());
                
                config.CreateMap<CreateUserInvitation.Types.Request, CreateUserInvitationCommand>();
                config.CreateMap<UpdateUserInvitation.Types.Request, UpdateUserInvitationCommand>();
                config.CreateMap<GetUserInvitationResult, GetUserInvitation.Types.Response>();
            });
            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}