using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile
{
    public class ProfileServiceErrors
    {
        public static InvalidProfileIdError InvalidProfileId() => new InvalidProfileIdError();        
        public static InvalidProfileParameterIdError InvalidProfileParameterId(int profileParameterId) => new InvalidProfileParameterIdError(profileParameterId);        
        public static ProfileForOrganizationUserAlreadyExistsError ProfileForOrganizationUserAlreadyExists() => new ProfileForOrganizationUserAlreadyExistsError();
        public static InvalidProfileValuesDataError InvalidProfileValuesData() => new InvalidProfileValuesDataError();
        public static InvalidOrganizationUserIdError InvalidOrganizationUserId() => new InvalidOrganizationUserIdError();
        public static InvalidOrganizationIdError InvalidOrganizationId() => new InvalidOrganizationIdError();

    }
}
