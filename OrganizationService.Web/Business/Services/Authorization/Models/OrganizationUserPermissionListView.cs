using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class OrganizationUserPermissionListView
    {
        public int? OrganizationUserId {get; set;}
        public string UserId { get; set; }
        public List<string> RoleIds {get; set;}
        public int? OrganizationId {get; set;}
        public string Permissions { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}