using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class GetOrganizationUserRolesResult
    {
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public ICollection<GetOrganizationUserRolesListView> Roles { get; set; }
    }
}
