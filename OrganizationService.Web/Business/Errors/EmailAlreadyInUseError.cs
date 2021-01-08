using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class EmailAlreadyInUseError : BaseError
    {
        public override string Code => "email_already_in_use";
    }
}