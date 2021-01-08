using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class QueryRolePermissionResult
    {
        public PaginationModel Pagination { get; set; }

        public ICollection<RolePermissionListView> Result { get; set; }
    }
}