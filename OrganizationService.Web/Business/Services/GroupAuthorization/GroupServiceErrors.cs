using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization
{
    public class GroupAuthorizationServiceErrors
    {
        public static InvalidQueryGroupAuthorizationDataError InvalidQueryGroupAuthorizationDataError(string fieldName = null) => new InvalidQueryGroupAuthorizationDataError(fieldName);
    }
}
