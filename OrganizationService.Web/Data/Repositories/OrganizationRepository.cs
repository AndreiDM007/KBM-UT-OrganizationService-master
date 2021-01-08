using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        #region members

        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        #endregion

        #region public API

        public OrganizationRepository(
            OrganizationServiceDataContext context,
            OrganizationDataMapper organizationDataMapper
        )
        {
            this.context = context;
            mapper = organizationDataMapper.Mapper;
        }

        public bool OrganizationExists(int id)
        {
            return GetValidOrganizations().Any(o => o.OrganizationEntityId == id);
        }

        public bool OrganizationExistsByName(string name)
        {
            return GetValidOrganizations().Any(o => o.Name == name);
        }

        public bool OrganizationExistsByName(string name, int organizationId)
        {
            return GetValidOrganizations().Any(o =>
                o.Name == name &&
                o.OrganizationEntityId != organizationId);
        }

        public void UpdateOrganization(UpdateOrganizationCommand command)
        {
            if (command.OrganizationId != null)
            {
                var id = command.OrganizationId.Value;
                var organizationEntity = context.Organizations.Find(id);

                organizationEntity.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (command.Name != null)
                {
                    organizationEntity.Name = command.Name;
                }
                if (command.IsActive.HasValue)
                {
                    organizationEntity.IsActive = command.IsActive.Value;
                }

                context.Organizations.Update(organizationEntity);
                context.SaveChanges();
            }
        }

        public void DeleteOrganization(int organizationId)
        {
            var organization = GetValidOrganizations()
                .Single(t => t.OrganizationEntityId == organizationId);

            organization.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            context.SaveChanges();
        }

        /**
         * Creates an organization
         */
        public int CreateOrganization(CreateOrganizationCommand command)
        {
            var organization = mapper.Map<OrganizationEntity>(command);
            organization.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.Organizations.Add(organization);
            this.context.SaveChanges();

            return organization.OrganizationEntityId;
        }

        public GetOrganizationResult GetOrganization(int organizationId)
        {
            var organization = GetValidOrganizations()
                .Single(t => t.OrganizationEntityId == organizationId);
            return new GetOrganizationResult
            {
                OrganizationId = organization.OrganizationEntityId,
                Name = organization.Name,
                IsActive = organization.IsActive
            };
        }

        public int? FindOrganizationIdByExternalUserId(string externalUserId)
        {
            var result = GetActiveOrganizations()
                .FirstOrDefault(o =>
                    o.OrganizationUsers.Any(ou =>
                        ou.UserId.Equals(
                            externalUserId, StringComparison.OrdinalIgnoreCase) && 
                            ou.RollbackedAt == null 
                            && ou.DeletedAt == null));

            return result?.OrganizationEntityId;
        }

        public void RollbackCreateOrganization(string transactionId)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                //TODO: There should be only one OrganizationUser for each CreateTenantSaga but you never know?

                var organizations = GetOrganizationsByTransactiondId(transactionId);
                if (organizations.Count > 0)
                {
                    foreach (var organization in organizations)
                    {
                        organization.RollbackedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }

                    this.context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        #endregion

        #region internal API

        private IQueryable<OrganizationEntity> GetValidOrganizations()
        {
            return this
                .context
                .Organizations
                .Where(g => g.RollbackedAt == null &&
                            g.DeletedAt == null);
        }
        
        private IQueryable<OrganizationEntity> GetActiveOrganizations()
        {
            return this
                .context
                .Organizations
                .Where(g => g.RollbackedAt == null &&
                            g.DeletedAt == null &&
                            g.IsActive);
        }

        private List<OrganizationEntity> GetOrganizationsByTransactiondId(string transactionId) =>
            this.context.Organizations.Where(t => t.TransactionId == transactionId && t.RollbackedAt == null).ToList();

        #endregion
    }
}