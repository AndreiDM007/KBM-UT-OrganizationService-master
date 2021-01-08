using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidProfileParameterIdError : BaseError
    {
        public override string Code => "invalid_profile_parameter_id";

        public int ProfileParameterId { get; }

        public InvalidProfileParameterIdError(int profileParameterId)
        {
            this.ProfileParameterId = profileParameterId;
        }
    }
}
