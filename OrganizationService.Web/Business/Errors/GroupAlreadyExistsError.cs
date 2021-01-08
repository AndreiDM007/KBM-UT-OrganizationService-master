using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class GroupAlreadyExistsError : BaseError
    {
        public override string Code => "group_already_exists";
    }
}
