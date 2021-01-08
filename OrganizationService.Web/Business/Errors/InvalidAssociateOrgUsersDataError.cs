using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidAssociateOrgUsersDataError : BaseError
    {
        public override string Code => "invalid_associate_org_users_data_error";
    }
}