using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class QueryGroupMemberResult
    {
        public PaginationModel Pagination { get; set; }
        public ICollection<GroupMemberListModel> Result { get; set; }
    }
}