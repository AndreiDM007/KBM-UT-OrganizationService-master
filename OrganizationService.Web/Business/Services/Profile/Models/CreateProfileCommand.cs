using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models
{
    public class CreateProfileCommand
    {
        public List<ProfileValueCreateModel> ProfileValues { get; set; }

        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }

        public string TransactionId { get; set; }
    }
}
