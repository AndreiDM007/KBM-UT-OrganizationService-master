using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models
{
    public class UpdateProfileCommand
    {
        public int ProfileId { get; set; }
        public int OrganizationUserId { get; set; }
        public int OrganizationId { get; set; }

        public List<ProfileValueUpdateModel> ProfileValues { get; set; }
    }
}