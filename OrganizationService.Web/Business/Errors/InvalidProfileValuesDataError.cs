using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidProfileValuesDataError : BaseError
    {
        public override string Code => "invalid_profile_values_data_error";
    }
}