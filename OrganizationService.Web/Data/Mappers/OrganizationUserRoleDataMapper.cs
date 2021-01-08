using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class OrganizationUserRoleDataMapper
    {
        public IMapper Mapper;

        public OrganizationUserRoleDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<CreateOrganizationUserRoleCommand, Entities.OrganizationUserRoleEntity>()
                    .ForMember(x => x.OrganizationUserRoleEntityId, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.CreatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())
                    .ForMember(x => x.OrganizationUser, x => x.Ignore())                    
                    ;
                config
                    .CreateMap<Entities.OrganizationUserRoleEntity, OrganizationUserRoleListView>()
                    ;
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}