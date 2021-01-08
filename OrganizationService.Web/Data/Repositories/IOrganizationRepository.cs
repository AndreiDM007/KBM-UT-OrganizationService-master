using Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IOrganizationRepository
    {
        bool OrganizationExists(int id);
        bool OrganizationExistsByName(string name);
        /// <summary>
        /// Tests if an organization name with the given string already exists, except for the one identified by ID
        /// </summary>
        /// <param name="name">Name to be found</param>
        /// <param name="organizationId">param to not check organization with given ID</param>
        /// <returns>true if an organization is found</returns>
        bool OrganizationExistsByName(string name, int organizationId);
        /// <summary>
        /// Update an organization
        /// </summary>
        /// <param name="command">model with ID and changed fields</param>
        void UpdateOrganization(UpdateOrganizationCommand command);
        int CreateOrganization(CreateOrganizationCommand command);

        GetOrganizationResult GetOrganization(int organizationId);
        void DeleteOrganization(int organizationId);
        int? FindOrganizationIdByExternalUserId(string externalUserId);
        void RollbackCreateOrganization(string transactionId);
    }
}