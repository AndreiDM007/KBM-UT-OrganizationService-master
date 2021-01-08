using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class QueryOrganizationUserCriteria
    {
        public ICollection<int> OrganizationIds { get; set; }
        public int UserType { get; set; }
    }
}