using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidOrganizationUserIdError : BaseError
    {
        public override string Code => nameof(InvalidOrganizationUserIdError).ToLowerSnakeCase();

        public InvalidOrganizationUserIdError()
        {
        }
    }
}