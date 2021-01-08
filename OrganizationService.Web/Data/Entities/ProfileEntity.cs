using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public partial class ProfileEntity
    {
        public ProfileEntity()
        {
            ProfileValues = new List<ProfileValueEntity>();
        }

        public int ProfileEntityId { get; set; }

        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }

        public OrganizationUserEntity OrganizationUser { get; set; }

        public IList<ProfileValueEntity> ProfileValues { get; set; }

        public string TransactionId { get; set; }
        
        public long? RollbackedAt { get; set; }
        
        public long? UpdatedAt { get; set; }
        
        public long? DeletedAt { get; set; }
    }

}
