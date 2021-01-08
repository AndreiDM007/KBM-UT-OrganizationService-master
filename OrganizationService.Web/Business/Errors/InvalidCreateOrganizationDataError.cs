using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidCreateOrganizationDataError : BaseError
    {
        public InvalidCreateOrganizationDataError(string field)
        {
            this.Field = field;
        }

        public override string Code => "invalid_create_organization_data_error";
    }
}

