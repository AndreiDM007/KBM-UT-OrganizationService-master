using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidOrganizationName : BaseError
    {
        public override string Code => nameof(InvalidOrganizationName).ToLowerSnakeCase();

        public InvalidOrganizationName()
        {
        }
    }
}