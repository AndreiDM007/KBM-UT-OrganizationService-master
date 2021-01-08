using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class NotFoundError : BaseError
    {
        public override string Code => "not_found_error";
    }
}
