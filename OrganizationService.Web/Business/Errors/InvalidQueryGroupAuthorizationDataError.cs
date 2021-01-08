using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidQueryGroupAuthorizationDataError : BaseError
    {
        public override string Code => "invalid_query_group_authorization_data_error";

        public InvalidQueryGroupAuthorizationDataError(string fieldName)
        {
            this.Field = fieldName;
        }
    }
}