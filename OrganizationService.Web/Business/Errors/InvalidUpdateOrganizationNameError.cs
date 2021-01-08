using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidUpdateOrganizationNameError : BaseError
    {
        public override string Code => "invalid_update_organization_name";

        public InvalidUpdateOrganizationNameError(string field)
        {
            Field = field;
        }
    }
}