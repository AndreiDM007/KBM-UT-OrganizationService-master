using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class QueryOrganizationUserResult
    {
        public int Total { get; set; }
        public ICollection<OrganizationUserListModel> OrganizationUsersData { get; set; }
    }
}