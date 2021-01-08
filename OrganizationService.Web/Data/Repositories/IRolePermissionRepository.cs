using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IRolePermissionRepository
    {
        bool ExistsRolePermission(int organizationId, string roleId);
        int CreateRolePermission(CreateRolePermissionCommand command);
        void UpdateRolePermission(UpdateRolePermissionCommand command);
        void DeleteRolePermission(DeleteRolePermissionCommand command);
        QueryRolePermissionResult QueryRolePermission(QueryRolePermissionCriteria model);
        List<OrganizationUserPermissionCustomModel> QueryRolePermissionByExternalUserId(int organizationId, string externalUserId);
    }
}