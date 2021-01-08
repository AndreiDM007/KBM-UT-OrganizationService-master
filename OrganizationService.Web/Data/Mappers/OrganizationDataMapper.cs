using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class OrganizationDataMapper
    {
        public IMapper Mapper;

        public OrganizationDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<CreateOrganizationCommand, OrganizationEntity>()
                    .ForMember(x => x.OrganizationEntityId, x => x.Ignore())
                    .ForMember(x => x.OrganizationUsers, x => x.Ignore())
                    .ForMember(x => x.RollbackedAt, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.CreatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())
                    ;
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}