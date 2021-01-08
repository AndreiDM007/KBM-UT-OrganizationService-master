using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class GetSingleOrganizationOrgUsersResult
    {
        public ICollection<SingleOrganizationOrgUserListModel> Result { get; set; }
    }
}