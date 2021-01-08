namespace Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models
{
    public class PaginationModel
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PaginationModel(int total, int page, int pageSize)
        {
            Total = total;
            Page = page;
            PageSize = pageSize;
        }
    }
}