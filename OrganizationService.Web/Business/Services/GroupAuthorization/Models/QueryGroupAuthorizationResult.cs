using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models
{
    public class QueryGroupAuthorizationResult
    {
        public int RequestOrganizationUserId { get; set; }
        public IEnumerable<GroupAuthorizationPermission> Permissions { get; set; }
        public int OrganizationId { get; set; }
    }
}
