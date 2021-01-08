using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidOrganizationIdError : BaseError
    {
        public override string Code => "invalid_organization_id_error";
    }
}