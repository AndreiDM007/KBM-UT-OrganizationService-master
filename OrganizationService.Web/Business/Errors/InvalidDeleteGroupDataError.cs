using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidDeleteGroupDataError : BaseError
    {
        public override string Code => "invalid_delete_group_data_error";

        public InvalidDeleteGroupDataError(string fieldName)
        {
            this.Field = fieldName;
        }
    }
}
