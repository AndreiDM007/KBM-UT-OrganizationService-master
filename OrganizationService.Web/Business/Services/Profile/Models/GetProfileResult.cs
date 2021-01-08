using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models
{
    public class GetProfileResult
    {
        public int ProfileId { get; set; }

        public List<ProfileValuesListModel> ProfileValues { get; set; }

        public int OrganizationUserId { get; set; }
    }
}
