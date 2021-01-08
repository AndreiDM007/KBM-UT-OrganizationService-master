using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class AssociationDoesNotExistsWithAnotherOrganizationUser : BaseError
    {
        public override string Code => "association_does_not_exists_with_another_organization_user";
    }
}