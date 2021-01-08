using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class OrganizationNotFoundError : BaseError
    {
        public override string Code => "organization_not_found_error";
    }
}