using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Kebormed.Core.Communication.Grpc.Extensions;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Mappers
{
    public class OrganizationGrpcMapper
    {
        public IMapper Mapper;

        public OrganizationGrpcMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                //incoming
                config.CreateMap<Int32Value, int?>().ConvertUsing(i => i.GetValue());
                config.CreateMap<StringValue, string>().ConvertUsing(i => i.GetValue());
                config.CreateMap<BoolValue, bool?>().ConvertUsing(i => i.GetValue());

                config.CreateMap<CreateOrganization.Types.Request, CreateOrganizationCommand>();
                config.CreateMap<UpdateOrganization.Types.Request, UpdateOrganizationCommand>();
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}