using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class AssociateOrganizationUserToGroupCommand
    {
        public int OrganizationId { get; set; }
        public int GroupId { get; set; }
        public int OrganizationUserId { get; set; }
        
        public ICollection<int> AllowedUserTypes { get; set; }
    }
}
