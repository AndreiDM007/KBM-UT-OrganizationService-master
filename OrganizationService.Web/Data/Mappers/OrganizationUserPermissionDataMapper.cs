using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class OrganizationUserPermissionDataMapper
    {
        public IMapper Mapper;

        public OrganizationUserPermissionDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {                
                config
                    .CreateMap<UpdateOrganizationUserPermissionCommand, Entities.OrganizationUserPermissionEntity>()
                    .ForMember(x => x.OrganizationUserPermissionEntityId, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.CreatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())
                    ;
                config
                    .CreateMap<Entities.OrganizationUserPermissionEntity, OrganizationUserPermissionListView>()
                    .ForMember(x => x.RoleIds, x => x.Ignore())
                    .ForMember(x => x.OrganizationId, x => x.Ignore())
                    ;
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}
