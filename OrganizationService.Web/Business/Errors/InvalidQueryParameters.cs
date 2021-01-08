using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidQueryParameters : BaseError
    {
        public override string Code => "invalid_query_parameters";

        public InvalidQueryParameters(string field)
        {
            Field = field;
        }
    }
}
