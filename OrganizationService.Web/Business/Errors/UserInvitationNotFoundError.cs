using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class UserInvitationNotFoundError : BaseError
    {
        public override string Code => "user_invitation_not_found_error";
    }
}