using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class ProfileForOrganizationUserAlreadyExistsError : BaseError
    {
        public override string Code => "profile_for_organization_user_already_exists_error";
    }
}