using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class OrganizationUserAndRolePermissionsListView
    {
        public int? OrganizationUserId { get; set; }
        public string UserId { get; set; }
        public int? OrganizationId { get; set; }
        /// <summary>
        /// This contains User and Multiple roles Permissions stored as Json.
        /// It will have to be converted into individual permissions in the layer above this service
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}