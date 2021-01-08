namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models
{
    public interface IQueryCriteria
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
        string Direction { get; set; }
        string OrderBy { get; set; }
    }
}