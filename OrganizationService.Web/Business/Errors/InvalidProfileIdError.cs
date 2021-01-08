using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidProfileIdError : BaseError
    {
        public override string Code => nameof(InvalidProfileIdError).ToLowerSnakeCase();

        public int? ProfileId { get; }

        public InvalidProfileIdError(int? profileId = null)
        {
            this.ProfileId = profileId;
        }
    }
}
