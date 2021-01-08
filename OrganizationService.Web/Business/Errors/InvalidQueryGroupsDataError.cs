using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidQueryGroupsDataError : BaseError
    {
        public override string Code => "invalid_query_group__data";

        public InvalidQueryGroupsDataError(string field)
        {
            Field = field;
        }
    }
}
