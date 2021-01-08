using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IOrganizationUserPermissionRepository
    {
        bool ExistsOrganizationUserPermission(int organizationUserId, string userId);
        int CreateOrganizationUserPermission(int organizationUserId, string userId, string permissions);
        void UpdateOrganizationUserPermission(UpdateOrganizationUserPermissionCommand command);        
        QueryOrganizationUserPermissionResult QueryOrganizationUserPermission(QueryOrganizationUserPermissionCriteria model);
        List<OrganizationUserPermissionCustomModel> QueryAllOrganizationUserPermission(int organizationId,
            string externalUserId);
    }
}