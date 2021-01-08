using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class AssociationOfTypeAlreadyExistsWithAnotherUser : BaseError
    {
        public override string Code => "association_of_type_already_exists_with_another_user";
    }
}