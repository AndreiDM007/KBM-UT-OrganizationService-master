using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidExternalUserIdError : BaseError
    {
        public override string Code => nameof(InvalidExternalUserIdError).ToLowerSnakeCase();
    }
}

