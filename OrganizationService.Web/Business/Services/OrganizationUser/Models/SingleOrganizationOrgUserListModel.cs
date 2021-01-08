namespace Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models
{
    public class SingleOrganizationOrgUserListModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        
        public int OrganizationId { get; set; }
        public int OrganizationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public int UserType { get; set; }

        public int NumEntities { get; set; }
    }
}