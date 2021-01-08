namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models
{
    public interface IProfileValueModel
    {
        string Value { get; set; }

        int ProfileValueId { get; set; }

        int ProfileParameterId { get; set; }

        string ProfileParameterName { get; set; }
    }
}