using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidGroupNameError : BaseError
    {
        public override string Code => "invalid_group_name_error";
    }
}
