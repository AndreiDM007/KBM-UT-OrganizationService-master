using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class RolePermissionDataMapper
    {
        public IMapper Mapper;

        public RolePermissionDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<CreateRolePermissionCommand, Entities.RolePermissionEntity>()
                    .ForMember(x => x.RolePermissionEntityId, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.CreatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())                    
                    ;
                config
                    .CreateMap<UpdateRolePermissionCommand, Entities.RolePermissionEntity>()
                    .ForMember(x => x.RolePermissionEntityId, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.CreatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())
                    .ForMember(x => x.IsDefault, x => x.Ignore())
                    ;
                config
                    .CreateMap<Entities.RolePermissionEntity, RolePermissionListView>()
                    ;
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}