using System;
using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class OrganizationUserRoleRepository : IOrganizationUserRoleRepository
    {

        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        public OrganizationUserRoleRepository(
            OrganizationServiceDataContext context,
            OrganizationUserRoleDataMapper organizationUserRoleDataMapper
        )
        {
            this.context = context;
            mapper = organizationUserRoleDataMapper.Mapper;
        }

        public bool ExistsOrganizationUserRole(int organizationUserId, int organizationId, string roleId)
        {
            return this.context.OrganizationUserRoles.Any(rp => rp.OrganizationId == organizationId
                                                                && rp.RoleId == roleId
                                                                && rp.OrganizationUserId == organizationUserId);
        }

        public int CreateOrganizationUserRole(CreateOrganizationUserRoleCommand command)
        {
            var entity = mapper.Map<OrganizationUserRoleEntity>(command);
            entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.OrganizationUserRoles.Add(entity);
            this.context.SaveChanges();

            return entity.OrganizationUserRoleEntityId;
        }

        public void DeleteOrganizationUserRole(DeleteOrganizationUserRoleCommand command)
        {
            var entity = this.context.OrganizationUserRoles.FirstOrDefault(rp => rp.OrganizationId == command.OrganizationId &&
                                                                                 rp.OrganizationUserId == command.OrganizationUserId &&
                                                                                 rp.RoleId == command.RoleId);
            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.OrganizationUserRoles.Update(entity);
            this.context.SaveChanges();
        }

        public QueryOrganizationUserRoleResult QueryOrganizationUserRole(QueryOrganizationUserRoleCriteria criteria)
        {
            if (criteria.Page == null || criteria.PageSize == null)
                return null;

            var pageSize = criteria.PageSize.Value;
            var pageValue = criteria.Page.Value;

            IQueryable<OrganizationUserRoleEntity> queryable = this.context.OrganizationUserRoles;

            if (!string.IsNullOrEmpty(criteria.RoleId))
            {
                queryable = queryable.Where(our => our.RoleId.Equals(criteria.RoleId));
            }

            if (criteria.OrganizationUserId.HasValue)
            {
                queryable = queryable.Where(our => our.OrganizationUserId.Equals(criteria.OrganizationUserId));
            }

            if (criteria.OrganizationId.HasValue)
            {
                queryable = queryable.Where(our => our.OrganizationId.Equals(criteria.OrganizationId));
            }

            var count = queryable.Count();

            int page = (pageValue - 1) * pageSize;

            var views = queryable
                .Skip(page)
                .Take(pageSize)
                .ToList();

            var results = views.Select(p => this.mapper.Map<OrganizationUserRoleListView>(p))
                .ToList();


            return new QueryOrganizationUserRoleResult
            {
                Pagination = new PaginationModel(count, pageValue, pageSize),
                Result = results
            };
        }

        public GetOrganizationUserRolesResult GetOrganizationUserRoles(int organizationUserId, int organizationId)
        {
            var roles = this
                .GetValidOrganizationUserRoles(organizationUserId, organizationId)
                .Join(context.RolePermissions,
                    organizationUserRoles => organizationUserRoles.RoleId,
                    rolePermissions => rolePermissions.RoleId,
                    (our, rp) => new OrganizationUserRoleCustomModel
                    {
                        RoleName = rp.RoleName,
                        Description = rp.Description,
                        IsDefault = rp.IsDefault,
                        DeletedAt = rp.DeletedAt,
                        DisplayName = rp.DisplayName
                    }
               ).Where(ourcm => ourcm.DeletedAt == null);


            var results = roles.Select(p => this.mapper.Map<GetOrganizationUserRolesListView>(p))
                .ToList();

            var result = new GetOrganizationUserRolesResult
            {
                OrganizationId = organizationId,
                OrganizationUserId = organizationUserId,
                Roles = results
            };

            return result;
        }

        private IQueryable<OrganizationUserRoleEntity> GetValidOrganizationUserRoles(int organizationUserId, int organizationId)
        {
            return context
                .OrganizationUserRoles
                .Where(our => our.OrganizationId == organizationId &&
                              our.OrganizationUserId == organizationUserId &&
                              our.DeletedAt == null);
        }

        private IQueryable<OrganizationUserRoleEntity> GetValidOrganizationUserRoles(int organizationId)
        {
            return context
                .OrganizationUserRoles
                .Where(gm => gm.OrganizationId == organizationId &&
                             gm.DeletedAt == null);
        }
    }
}