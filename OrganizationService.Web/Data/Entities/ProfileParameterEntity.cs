using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public partial class ProfileParameterEntity
    {
        public ProfileParameterEntity()
        {
            ProfileValues = new HashSet<ProfileValueEntity>();
        }

        public int ProfileParameterEntityId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProfileValueEntity> ProfileValues { get; set; }
    }
}
