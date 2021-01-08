using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IOrganizationUserRoleRepository
    {
        int CreateOrganizationUserRole(CreateOrganizationUserRoleCommand command);
        void DeleteOrganizationUserRole(DeleteOrganizationUserRoleCommand command);
        QueryOrganizationUserRoleResult QueryOrganizationUserRole(QueryOrganizationUserRoleCriteria model);
        bool ExistsOrganizationUserRole(int organizationUserId, int organizationId, string roleId);
        GetOrganizationUserRolesResult GetOrganizationUserRoles(int organizationUserId, int organizationId);
    }
}