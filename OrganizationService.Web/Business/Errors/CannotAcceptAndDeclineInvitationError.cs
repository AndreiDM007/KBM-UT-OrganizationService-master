using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class CannotAcceptAndDeclineInvitationError : BaseError
    {
        public override string Code => "cannot_accept_and_decline_invitation_error";
    }
}