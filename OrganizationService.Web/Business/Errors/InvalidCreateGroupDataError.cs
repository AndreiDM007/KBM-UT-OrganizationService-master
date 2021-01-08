using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidCreateGroupDataError : BaseError
    {
        public override string Code => "invalid_create_group_data_error";

        public InvalidCreateGroupDataError(string fieldName)
        {
            this.Field = fieldName;
        }
    }
}
