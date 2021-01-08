using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Organization
{
    public class OrganizationServiceErrors
    {
        public static OrganizationAlreadyExistsError OrganizationAlreadyExistsError() => new OrganizationAlreadyExistsError();
        public static InvalidCreateOrganizationDataError InvalidCreateOrganizationDataError(string field) => new InvalidCreateOrganizationDataError(field);
        public static InvalidExternalUserIdError InvalidExternalUserId() => new InvalidExternalUserIdError();
        public static InvalidOrganizationIdError InvalidOrganizationIdError() => new InvalidOrganizationIdError();
        public static InvalidUpdateOrganizationNameError InvalidUpdateOrganizationNameError(string field) => new InvalidUpdateOrganizationNameError(field);
        public static OrganizationNotFoundError OrganizationNotFoundError() => new OrganizationNotFoundError();
    }
}