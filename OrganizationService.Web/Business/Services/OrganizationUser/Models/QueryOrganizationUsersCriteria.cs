using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class QueryOrganizationUsersCriteria
    {
        public int OrganizationId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Q { get; set; }
        public string OrderBy { get; set; }
        public string Direction { get; set; }
        public bool? IsActive { get; set; }
        public List<int> UserTypes { get; set; }
    }
}
