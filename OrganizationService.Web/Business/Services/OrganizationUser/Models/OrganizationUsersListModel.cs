using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class OrganizationUsersListModel
    {
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }        
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }        
        public int UserType { get; set; }
        public ICollection<RoleListModel> Roles { get; set; }
        public bool IsPendingActivation { get; set; }
        public long? LastLoginTime { get; set; }
        public long? CreatedAt { get; set; }
        public bool IsLocked { get; set; }
    }
}
