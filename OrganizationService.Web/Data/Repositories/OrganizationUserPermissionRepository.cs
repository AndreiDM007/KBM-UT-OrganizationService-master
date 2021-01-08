using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class OrganizationUserPermissionRepository : IOrganizationUserPermissionRepository
    {
        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        public OrganizationUserPermissionRepository(
            OrganizationServiceDataContext context,
            OrganizationUserPermissionDataMapper organizationUserPermissionDataMapper)
        {
            this.context = context;
            this.mapper = organizationUserPermissionDataMapper.Mapper;
        }

        public bool ExistsOrganizationUserPermission(int organizationUserId, string userId)
        {
            return this.context.OrganizationUserPermissions.Any(rp => rp.OrganizationUserId == organizationUserId && rp.UserId == userId);
        }

        public int CreateOrganizationUserPermission(int organizationUserId, string userId, string permissions)
        {
            var entity = new OrganizationUserPermissionEntity
            {
                OrganizationUserId = organizationUserId,
                UserId = userId,
                Permissions = permissions,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            this.context.OrganizationUserPermissions.Add(entity);
            this.context.SaveChanges();

            return entity.OrganizationUserPermissionEntityId;
        }
        
        public void UpdateOrganizationUserPermission(UpdateOrganizationUserPermissionCommand command)
        {
            var entity = GetOrganizationUserPermission(command.OrganizationUserId.Value);
            entity.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (!string.IsNullOrWhiteSpace(command.Permissions))
            {
                entity.Permissions = command.Permissions;
            }

            context.OrganizationUserPermissions.Update(entity);
            this.context.SaveChanges();
        }

        public QueryOrganizationUserPermissionResult QueryOrganizationUserPermission(
            QueryOrganizationUserPermissionCriteria criteria)
        {
            if (criteria.Page == null || criteria.PageSize == null)
                return null;

            var pageSize = criteria.PageSize.Value;
            var pageValue = criteria.Page.Value;

            IQueryable<OrganizationUserPermissionEntity> queryable = this.context.OrganizationUserPermissions;


            if (criteria.OrganizationUserId.HasValue)
            {
                queryable = queryable.Where(our => our.OrganizationUserId.Equals(criteria.OrganizationUserId));
            }

            var count = queryable.Count();

            int page = (pageValue - 1) * pageSize;

            var views = queryable
                .Skip(page)
                .Take(pageSize)
                .ToList();

            var results = views.Select(p => this.mapper.Map<OrganizationUserPermissionListView>(p))
                .ToList();


            return new QueryOrganizationUserPermissionResult
            {
                Pagination = new PaginationModel(count, pageValue, pageSize),
                Result = results
            };

        }

        public List<OrganizationUserPermissionCustomModel> QueryAllOrganizationUserPermission(int organizationId, string externalUserId)
            {
                var result = this.context.OrganizationUsers
                    .Where(ou => ou.OrganizationId.Equals(organizationId) && 
                                 ou.UserId.Equals(externalUserId, StringComparison.InvariantCultureIgnoreCase))
                    .Join(context.OrganizationUserPermissions,
                        organizationUserRoles => organizationUserRoles.OrganizationUserEntityId,
                        userPermissions => userPermissions.OrganizationUserId,
                        (our, rp) => new OrganizationUserPermissionCustomModel
                        {
                            OrganizationId = our.OrganizationId,
                            OrganizationUserId = our.OrganizationUserEntityId,
                            ExternalUserId = our.UserId,
                            Permissions = rp.Permissions,
                            DeletedAt = rp.DeletedAt
                        }
                    )
                    .Where(ourpcm => ourpcm.DeletedAt == null);


                return result.ToList();
            }

            private OrganizationUserPermissionEntity GetOrganizationUserPermission(int organizationUserId)
        {
            return this.context.OrganizationUserPermissions.FirstOrDefault(rp => rp.OrganizationUserId == organizationUserId);
        }
    }
}