namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models
{
    public class ProfileValueUpdateModel : IProfileValueModel
    {
        public string Value { get; set; }

        public int ProfileValueId { get; set; }

        public int ProfileParameterId { get; set; }

        public string ProfileParameterName { get; set; }
    }
}