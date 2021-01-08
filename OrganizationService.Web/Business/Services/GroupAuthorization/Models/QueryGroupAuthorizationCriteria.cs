using System.Collections.Generic;
using System.Data;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models
{
    public class QueryGroupAuthorizationCriteria
    {
        public int? RequestOrganizationUserId { get; set; }
        public IEnumerable<int> TargetOrganizationUserIdCollection { get; set; }
        public int? OrganizationId { get; set; }
    }
}