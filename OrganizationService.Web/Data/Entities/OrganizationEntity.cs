using System.Collections.Generic;

namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public class OrganizationEntity
    {
        public OrganizationEntity()
        {
            OrganizationUsers = new HashSet<OrganizationUserEntity>();
        }

        public int OrganizationEntityId { get; set; }

        public string Name { get; set; }
        
        public bool IsActive { get; set; }

        public ICollection<OrganizationUserEntity> OrganizationUsers { get; set; }

        public string TransactionId { get; set; }
        public long? RollbackedAt { get; set; }
        public long? DeletedAt { get; set; }
        public long? UpdatedAt { get; set; }
        public long? CreatedAt { get; set; }
    }
}
