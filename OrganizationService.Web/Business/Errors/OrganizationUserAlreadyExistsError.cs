using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class OrganizationUserAlreadyExistsError : BaseError
    {
        public override string Code => nameof(OrganizationUserAlreadyExistsError).ToLowerSnakeCase();

        public OrganizationUserAlreadyExistsError()
        {
        }
    }
}