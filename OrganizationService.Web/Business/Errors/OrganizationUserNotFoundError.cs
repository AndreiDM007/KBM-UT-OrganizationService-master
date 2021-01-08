using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class OrganizationUserNotFoundError : BaseError
    {
        public override string Code => "organization_user_not_found_error";
    }
}
