using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidAssociateOrganizationUserToGroupDataError : BaseError
    {
        public override string Code => "invalid_associate_organization_user_to_group_data_error";

        public InvalidAssociateOrganizationUserToGroupDataError(string fieldName)
        {
            this.Field = fieldName;
        }
    }
}
