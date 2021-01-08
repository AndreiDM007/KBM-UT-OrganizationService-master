using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization
{
    public class AuthorizationServiceErrors
    {
        public static EntityAlreadyExistsError EntityAlreadyExistsError(string field) => new EntityAlreadyExistsError(field);
        public static InvalidCreateOrganizationDataError InvalidCreateOrganizationDataError(string field) => new InvalidCreateOrganizationDataError(field);
        public static InvalidExternalUserIdError InvalidExternalUserId() => new InvalidExternalUserIdError();
        public static InvalidOrganizationIdError InvalidOrganizationIdError() => new InvalidOrganizationIdError();

        public static InvalidUpdateOrganizationNameError InvalidUpdateOrganizationNameError(string field) => new InvalidUpdateOrganizationNameError(field);
        public static InvalidDeleteEntityError InvalidDeleteEntityError(string field) => new InvalidDeleteEntityError(field);
    }
}