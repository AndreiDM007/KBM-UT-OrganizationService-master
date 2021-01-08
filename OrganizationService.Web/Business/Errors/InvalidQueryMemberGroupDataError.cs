using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidQueryMemberGroupDataError : BaseError
    {
        public override string Code => "invalid_query_member_group_data";

        public InvalidQueryMemberGroupDataError(string field)
        {
            Field = field;
        }
    }
}