using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class QueryGroupMemberCriteria
    {
        public int GroupId { get; set; }
        public int OrganizationId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }        
        public string OrderBy { get; set; }
        public string Direction { get; set; }
        
        public ICollection<int> TargetUserTypes { get; set; }
    }
}