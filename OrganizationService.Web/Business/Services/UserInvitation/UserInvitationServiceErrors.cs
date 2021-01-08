using Humanizer;
using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation
{
    public class UserInvitationServiceErrors
    {
        public static InvalidOrganizationIdError InvalidOrganizationIdError() => new InvalidOrganizationIdError();
        
        public static OrganizationNotFoundError OrganizationNotFoundError() => new OrganizationNotFoundError();
        
        public static InvalidExternalUserIdError InvalidExternalUserIdError() => new InvalidExternalUserIdError();
        public static InvalidOrganizationUserIdError InvalidOrganizationUserIdError() => new InvalidOrganizationUserIdError();
        
        public static OrganizationUserNotFoundError OrganizationUserNotFoundError() => new OrganizationUserNotFoundError();
        
        public static InvalidUserInviteGuidError InvalidUserInviteGuidError() => new InvalidUserInviteGuidError();
        
        public static CannotAcceptAndDeclineInvitationError CannotAcceptAndDeclineInvitationError() => new CannotAcceptAndDeclineInvitationError();
        
        public static UserInvitationNotFoundError UserInvitationNotFoundError() => new UserInvitationNotFoundError();
    }
}