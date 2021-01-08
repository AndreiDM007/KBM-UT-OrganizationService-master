using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidUserInviteGuidError : BaseError
    {
        public override string Code => "invalid_user_invite_guid_error";
    }
}