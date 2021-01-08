using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidDeleteEntityError : BaseError
    {
        public override string Code => "invalid_delete_entity_error";

        public InvalidDeleteEntityError(string field)
        {
            Field = field;

        }
    }
}