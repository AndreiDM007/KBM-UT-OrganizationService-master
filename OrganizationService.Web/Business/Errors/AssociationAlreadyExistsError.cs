using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class AssociationAlreadyExistsError : BaseError
    {
        public override string Code => "association_already_exists";
    }
}