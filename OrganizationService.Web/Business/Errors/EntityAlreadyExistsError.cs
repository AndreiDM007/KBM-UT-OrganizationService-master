using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class EntityAlreadyExistsError : BaseError
    {
        public override string Code => "entity_already_exists";

        public EntityAlreadyExistsError(string field)
        {
            Field = field;

        }
    }
}