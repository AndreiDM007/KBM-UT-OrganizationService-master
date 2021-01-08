namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class GroupMemberListModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long CreationTime { get; set; }
        public int UserType { get; set; }
        public int OrganizationUserId { get; set; }
    }
}