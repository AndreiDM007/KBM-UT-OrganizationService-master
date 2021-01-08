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
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        private const string SortIsDefault = "isdefault";
        private const string SortDisplayName = "displayname";
        public static readonly string[] SortableFields =
        {
            SortIsDefault,
            SortDisplayName
        };

        public RolePermissionRepository(
            OrganizationServiceDataContext context,
            RolePermissionDataMapper rolePermissionDataMapper
        )
        {
            this.context = context;
            this.mapper = rolePermissionDataMapper.Mapper;
        }

        public bool ExistsRolePermission(int organizationId, string roleId) =>
            this.context.RolePermissions.Any(rp => rp.OrganizationId == organizationId && rp.RoleId == roleId);

        public int CreateRolePermission(CreateRolePermissionCommand command)
        {

            var entity = mapper.Map<RolePermissionEntity>(command);
            entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.RolePermissions.Add(entity);
            this.context.SaveChanges();

            return entity.RolePermissionEntityId;
        }

        public void UpdateRolePermission(UpdateRolePermissionCommand command)
        {
            var entity = GetRolePermission(command.OrganizationId.Value, command.RoleId);
            entity.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (!string.IsNullOrWhiteSpace(command.RoleName))
            {
                entity.RoleName = command.RoleName;
            }

            if (!string.IsNullOrWhiteSpace(command.Permissions))
            {
                entity.Permissions = command.Permissions;
            }

            if (!string.IsNullOrWhiteSpace(command.Description))
            {
                entity.Description = command.Description;
            }           

            context.RolePermissions.Update(entity);
            this.context.SaveChanges();
        }

        public void DeleteRolePermission(DeleteRolePermissionCommand command)
        {
            var entity = GetRolePermission(command.OrganizationId.Value, command.RoleId);
            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            context.RolePermissions.Update(entity);
            this.context.SaveChanges();
        }

        public QueryRolePermissionResult QueryRolePermission(QueryRolePermissionCriteria criteria)
        {
            if (criteria.Page == null || criteria.PageSize == null)
                return null;

            var pageSize = criteria.PageSize.Value;
            var pageValue = criteria.Page.Value;

            IQueryable<RolePermissionEntity> queryable = this.context.RolePermissions;

            if (!string.IsNullOrEmpty(criteria.RoleId))
            {
                queryable = queryable.Where(our => our.RoleId.Equals(criteria.RoleId));
            }

            if (!string.IsNullOrEmpty(criteria.RoleName))
            {
                queryable = queryable.Where(our => our.RoleName.Equals(criteria.RoleName));
            }

            if (criteria.OrganizationId.HasValue)
            {
                queryable = queryable.Where(our => our.OrganizationId.Equals(criteria.OrganizationId));
            }

            if (!string.IsNullOrWhiteSpace(criteria.OrderBy))
            {
                var descending = criteria.Direction.ToUpper() == "DESC";
                var orderByField = criteria.OrderBy.ToLowerInvariant();

                switch (orderByField)
                {
                    case SortIsDefault:
                        queryable = queryable.OrderBy(r => !r.IsDefault, descending);
                        break;
                    case SortDisplayName:
                        queryable = queryable.OrderBy(r => r.DisplayName, descending);
                        break;                 
                    default:
                        queryable = queryable.OrderBy(r => r.DisplayName, false);
                        break;
                }
            }

            var count = queryable.Count();

            var page = (pageValue - 1) * pageSize;

            var views = queryable
                .Skip(page)
                .Take(pageSize)
                .ToList();

            var results = views.Select(p => this.mapper.Map<RolePermissionListView>(p))
                .ToList();


            return new QueryRolePermissionResult
            {
                Pagination = new PaginationModel(count, pageValue, pageSize),
                Result = results
            };
        }

        public List<OrganizationUserPermissionCustomModel> QueryRolePermissionByExternalUserId(int organizationId, string externalUserId)
        {
            var result = this.context.OrganizationUserRoles
                .Where(rp => rp.DeletedAt == null && rp.OrganizationId == organizationId)
                .Include(rp => rp.OrganizationUser)
                .Where(rp => rp.OrganizationUser.DeletedAt == null
                             && rp.OrganizationUser.RollbackedAt == null
                             && rp.OrganizationUser.UserId == externalUserId)
                .Join(context.RolePermissions,
                    organizationUserRoles => organizationUserRoles.RoleId,
                    rolePermissions => rolePermissions.RoleId,
                    (our, rp) => new OrganizationUserPermissionCustomModel
                    {
                        OrganizationId = our.OrganizationUser.OrganizationId,
                        OrganizationUserId = our.OrganizationUserId,
                        ExternalUserId = our.OrganizationUser.UserId,
                        Permissions = rp.Permissions,
                        DeletedAt = rp.DeletedAt
                    }
                )
                .Where(ourpcm => ourpcm.DeletedAt == null);

            return result.ToList();
        }

        private RolePermissionEntity GetRolePermission(int organizationId, string roleId) =>
            this.context.RolePermissions.FirstOrDefault(rp => rp.OrganizationId == organizationId
                                                              && rp.RoleId == roleId);
    }
}