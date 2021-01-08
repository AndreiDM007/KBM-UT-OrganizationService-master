using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class QueryOrganizationUsersResult
    {
        public PaginationModel Pagination { get; set; }
        public ICollection<OrganizationUsersListModel> Result { get; set; }
    }
}
