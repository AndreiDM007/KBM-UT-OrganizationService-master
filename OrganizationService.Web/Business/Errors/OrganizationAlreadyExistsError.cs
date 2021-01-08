using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class OrganizationAlreadyExistsError : BaseError
    {
        public override string Code => "organization_already_exists";

    }
}