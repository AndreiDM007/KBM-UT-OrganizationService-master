using AutoMapper;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Mappers
{
    public class ProfileGrpcMapper
    {
        public IMapper Mapper { get; }

        public ProfileGrpcMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProfileValuesListModel, ProfileValue>()
                .ForMember(x => x.TransactionId, x => x.Ignore())
                ;
            });
            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}