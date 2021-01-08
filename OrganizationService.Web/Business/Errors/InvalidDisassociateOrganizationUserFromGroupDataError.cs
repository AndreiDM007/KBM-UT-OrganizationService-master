using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidDisassociateOrganizationUserFromGroupDataError : BaseError
    {
        public override string Code => "invalid_disassociate_organization_user_from_group_data_error";

        public InvalidDisassociateOrganizationUserFromGroupDataError()
        {
        }

        public InvalidDisassociateOrganizationUserFromGroupDataError(string fieldName)
        {
            this.Field = fieldName;
        }
    }
}