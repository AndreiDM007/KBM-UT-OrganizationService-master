namespace Kebormed.Core.OrganizationService.Web.Data.Entities
{
    public partial class ProfileValueEntity
    {
        public int ProfileValueEntityId { get; set; }
        public int ProfileParameterId { get; set; }
        public int ProfileId { get; set; }
        public string Value { get; set; }

        public virtual ProfileEntity Profile { get; set; }
        public virtual ProfileParameterEntity ProfileParameter { get; set; }

        public string TransactionId { get; set; }
        public long? RollbackedAt { get; set; }        
    }
}
