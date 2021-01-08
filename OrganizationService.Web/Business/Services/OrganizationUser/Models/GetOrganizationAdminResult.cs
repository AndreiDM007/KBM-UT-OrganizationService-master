using System;
using System.Collections.Generic;
using System.Text;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class GetOrganizationAdminResult
    {
        public int OrganizationUserId { get; set; }    
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }        
        public long? CreatedAt { get; set; }
    }
}
