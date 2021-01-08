using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidUserEmailError : BaseError
    {
        public override string Code => "invalid_user_email_error";
    }

    public class InvalidUsernameError : BaseError
    {
        public override string Code => "invalid_username_error";
    }
}
