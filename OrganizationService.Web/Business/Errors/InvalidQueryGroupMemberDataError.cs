using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidQueryGroupMemberDataError : BaseError
    {
        public override string Code => "invalid_query_group_member_data";

        public InvalidQueryGroupMemberDataError(string field)
        {
            Field = field;
        }
    }
}