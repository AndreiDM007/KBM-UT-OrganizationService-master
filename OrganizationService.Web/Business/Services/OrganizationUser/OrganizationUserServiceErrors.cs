using Kebormed.Core.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser
{
    public class OrganizationUserServiceErrors
    {
        public static InvalidQueryParameters InvalidQueryParameters(string field) => new InvalidQueryParameters(field);
        public static InvalidProfileIdError InvalidProfileId(int profileId) => new InvalidProfileIdError(profileId);
        public static OrganizationUserAlreadyExistsError OrganizationUserAlreadyExists() => new OrganizationUserAlreadyExistsError();
        public static InvalidOrganizationUserIdError InvalidOrganizationUserValuesData() => new InvalidOrganizationUserIdError();
        public static InvalidAssociateOrgUsersDataError InvalidAssociateOrgUsersData() => new InvalidAssociateOrgUsersDataError();
        public static InvalidOrganizationUserIdError InvalidOrganizationUserId() => new InvalidOrganizationUserIdError();
        public static AssociationAlreadyExistsError AssociationAlreadyExists() => new AssociationAlreadyExistsError();
        public static AssociationOfTypeAlreadyExistsWithAnotherUser AssociationOfTypeAlreadyExistsWithAnotherUser() => new AssociationOfTypeAlreadyExistsWithAnotherUser();
        public static AssociationDoesNotExistsWithAnotherOrganizationUser AssociationDoesNotExistsWithAnotherOrganizationUser() => new AssociationDoesNotExistsWithAnotherOrganizationUser();
        public static InvalidOrganizationIdError InvalidOrganizationId() => new InvalidOrganizationIdError();
        public static InvalidUserTypeIdError InvalidUserTypeId() => new InvalidUserTypeIdError();
        public static NotFoundError NotFoundError() => new NotFoundError();
        public static EmailAlreadyInUseError EmailAlreadyInUseError() => new EmailAlreadyInUseError();
        public static InvalidUserTypeIdError InvalidUserTypeIdError() => new InvalidUserTypeIdError();
        public static InvalidOrganizationName InvalidOrganizationName() => new InvalidOrganizationName();
        public static InvalidExternalUserIdError InvalidExternalUserIdError() => new InvalidExternalUserIdError();
    }
}
