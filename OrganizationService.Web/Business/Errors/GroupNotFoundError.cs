using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class GroupNotFoundError : BaseError
    {
        public override string Code => "group_not_found_error";
    }
}
