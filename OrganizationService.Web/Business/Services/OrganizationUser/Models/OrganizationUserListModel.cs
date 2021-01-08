using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class OrganizationUserListModel
    {
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int ProfileId { get; set; }
        public ICollection<ProfileValuesListModel> ProfileValues { get; set; }
    }
}
