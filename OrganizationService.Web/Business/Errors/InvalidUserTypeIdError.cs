using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidUserTypeIdError : BaseError
    {
        public override string Code => nameof(InvalidUserTypeIdError).ToLowerSnakeCase();

        public InvalidUserTypeIdError()
        {
        }
    }
}
