using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IGroupAuthorizationRepository
    {
        List<int> QueryGroupAuthorization(QueryGroupAuthorizationCriteria command);
       
    }
}
