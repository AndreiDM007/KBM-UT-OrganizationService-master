namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public class QueryOrganizationUserRoleCriteria : IQueryCriteria
    {
        public int? OrganizationUserId {get; set;}
        public string RoleId {get; set;}
        public int? OrganizationId {get; set;}
        public int? Page {get; set;}
        public int? PageSize {get; set;}
        public string Direction {get; set;}
        public string OrderBy {get; set;}
    }
}