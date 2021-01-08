using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Kebormed.Core.Communication.Grpc.Extensions;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Mappers
{
    public class AuthorizationGrpcMapper
    {
        public IMapper Mapper;

        public AuthorizationGrpcMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                //incoming
                config.CreateMap<Int32Value, int?>().ConvertUsing(i => i.GetValue());
                config.CreateMap<StringValue, string>().ConvertUsing(i => i.GetValue());
                                
                config.CreateMap<UpdateOrganizationUserPermission.Types.Request, UpdateOrganizationUserPermissionCommand>();                
                config.CreateMap<QueryOrganizationUserPermissionResult, QueryOrganizationUserPermission.Types.Response>();
                config
                    .CreateMap<OrganizationUserPermissionListView,
                        QueryOrganizationUserPermission.Types.OrganizationUserPermissionListView>();
                    


                config.CreateMap<CreateOrganizationUserRole.Types.Request, CreateOrganizationUserRoleCommand>();
                config.CreateMap<DeleteOrganizationUserRole.Types.Request, DeleteOrganizationUserRoleCommand>();
                config.CreateMap<QueryOrganizationUserRoleResult, QueryOrganizationUserRole.Types.Response>();

                config.CreateMap<OrganizationUserRoleListView, QueryOrganizationUserRole.Types.OrganizationUserRoleListView>();


                config.CreateMap<CreateRolePermission.Types.Request, CreateRolePermissionCommand>();
                config.CreateMap<UpdateRolePermission.Types.Request, UpdateRolePermissionCommand>();
                config.CreateMap<DeleteRolePermission.Types.Request, DeleteRolePermissionCommand>();
                config.CreateMap<QueryRolePermissionResult, QueryRolePermission.Types.Response>();
                config.CreateMap<RolePermissionListView, QueryRolePermission.Types.RolePermissionListView>();

                config.CreateMap<PaginationModel, QueryOrganizationUserPermission.Types.Pagination>();
                config.CreateMap<PaginationModel, QueryOrganizationUserRole.Types.Pagination>();
                config.CreateMap<PaginationModel, QueryRolePermission.Types.Pagination>();
                config.CreateMap<QueryOrganizationUserPermission.Types.Request, QueryOrganizationUserPermissionCriteria>();
                
                });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}