using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class OrganizationUserRepository : IOrganizationUserRepository
    {
        #region members

        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        #endregion

        #region public API: OrganizationUser

        public OrganizationUserRepository(OrganizationServiceDataContext context, OrganizationUserDataMapper organizationUserDataMapper)
        {
            this.context = context;
            this.mapper = organizationUserDataMapper.Mapper;
        }

        /// <inheritdoc />
        public bool OrganizationUserExists(int organizationUserId, int organizationId) =>
            this.GetBaseOrganizationUserEntity()
                .Any(ou => ou.OrganizationId == organizationId && organizationUserId == ou.OrganizationUserEntityId);

        /// <inheritdoc />
        public bool OrganizationUserExists(int organizationUserId, int userType, int organizationId) =>
            this.GetBaseOrganizationUserEntity()
                .Any(ou => ou.OrganizationUserEntityId == organizationUserId && ou.OrganizationId == organizationId && ou.UserType == userType);

        /// <inheritdoc />
        public bool OrganizationUserExists(string userId, int userType, int organizationId) =>
            this.GetBaseOrganizationUserEntity()
                .Any(ou => ou.UserId == userId && ou.OrganizationId == organizationId && ou.UserType == userType);

        /// <inheritdoc />
        public bool OrganizationUserExistsByEmail(string email) =>
            this.GetBaseOrganizationUserEntity()
                .Include(ou => ou.Organization)
                .Where(ou => ou.Organization.DeletedAt == null && ou.Organization.RollbackedAt == null)
                .Any(ou => ou.Email.Equals(email));
        /// <inheritdoc />
        public bool OrganizationUserExistsByUsername(string username) =>
            this.GetBaseOrganizationUserEntity()
                .Include(ou => ou.Organization)
                .Where(ou => ou.Organization.DeletedAt == null && ou.Organization.RollbackedAt == null)
                .Any(ou => ou.Username.Equals(username));

        public int? GetOrganizationUserType(int organizationUserId, int organizationId)
        {
            var result = GetOrganizationUserEntity(organizationUserId, organizationId);

            return result?.UserType;
        }

        public GetOrganizationUserResult GetOrganizationUser(int organizationUserId, int userType, int organizationId)
        {
            var result = GetOrganizationUserEntity(organizationUserId, userType, organizationId);

            if (result is null)
            {
                return null;
            }

            return mapper.Map<GetOrganizationUserResult>(result);
        }

        public GetOrganizationUserByExternalUserIdResult GetOrganizationUserByExternalUserId(int organizationId, string userId)
        {
            var result = GetBaseOrganizationUserEntity().FirstOrDefault(t => t.OrganizationId.Equals(organizationId) && t.UserId.Equals(userId));

            if (result is null)
            {
                return null;
            }

            return mapper.Map<GetOrganizationUserByExternalUserIdResult>(result);
        }

        public GetOrganizationAdminResult GetOrganizationAdmin(int organizationId, int userType)
        {
            var result = this.GetBaseOrganizationUserEntity().FirstOrDefault(p => p.OrganizationId == organizationId && p.UserType == userType);

            if (result is null)
            {
                return null;
            }

            return mapper.Map<GetOrganizationAdminResult>(result);
        }

        public int CreateOrganizationUser(CreateOrganizationUserCommand model)
        {
            var organizationUser = mapper.Map<OrganizationUserEntity>(model);
            organizationUser.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            using (var transaction = this.context.Database.BeginTransaction())
            {
                this.context.OrganizationUsers.Add(organizationUser);
                this.context.SaveChanges();
                transaction.Commit();
            }

            return organizationUser.OrganizationUserEntityId;
        }

        public void UpdateOrganizationUser(UpdateOrganizationUserCommand command)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Update old entry at ObsoletedAt                 
            var organizationUserEntity = GetOrganizationUserEntity(command.OrganizationUserId, command.UserType, command.OrganizationId);
            organizationUserEntity.UpdatedAt = now;
            if (command.Email != null)
            {
                organizationUserEntity.Email = command.Email;
            }

            if (command.FirstName != null)
            {
                organizationUserEntity.FirstName = command.FirstName;
            }

            if (command.LastName != null)
            {
                organizationUserEntity.LastName = command.LastName;
            }

            if (command.IsActive != null)
            {
                organizationUserEntity.IsActive = command.IsActive.Value;
            }
            
            context.OrganizationUsers.Update(organizationUserEntity);
            this.context.SaveChanges();
        }

        public long DeleteOrganizationUser(int organizationUserId, int userType, int organizationId)
        {
            var organizationUserEntity = this.GetOrganizationUserEntity(organizationUserId, userType, organizationId);
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            organizationUserEntity.DeletedAt = now;
            //delete associations
            var allAssociations = GetAllAssociationsWithUser(organizationUserId).ToList();
            allAssociations.ForEach(a => a.DeletedAt = now);
            using (var transaction = this.context.Database.BeginTransaction())
            {
                context.AssociatedOrganizationUserEntities.UpdateRange(allAssociations);
                this.context.OrganizationUsers.Update(organizationUserEntity);
                this.context.SaveChanges();
                transaction.Commit();
            }
            return organizationUserEntity.DeletedAt.Value;
        }

        public AnyOrganizationUserWithSharedUserId AnyOrganizationUserWithSharedUserId(int organizationUserId)
        {
            var organizationUserEntity = this.context.OrganizationUsers.FirstOrDefault(p => p.OrganizationUserEntityId == organizationUserId);
            var isLastUserIdRelationship = !this.GetBaseOrganizationUserEntity().Any(t => t.UserId == organizationUserEntity.UserId);
            return new AnyOrganizationUserWithSharedUserId
            {
                IsLastUserIdRelationship = isLastUserIdRelationship,
                UserId = organizationUserEntity.UserId
            };
        }

        public GetSingleOrganizationOrgUsersResult GetSingleOrganizationOrgUsers(int organizationId)
        {
            var outerSelection = GetBaseOrganizationUserEntity();
            var singleEntityUsers = GetBaseOrganizationUserEntity()
                .Where(ou => ou.OrganizationId == organizationId)
                .GroupJoin(
                    outerSelection,
                    ou1 => ou1.UserId,
                    ou2 => ou2.UserId,
                    (ou1, outer) => new SingleOrganizationOrgUserListModel
                    {
                        Email = ou1.Email,
                        UserId = ou1.UserId,
                        OrganizationId = ou1.OrganizationId,
                        OrganizationUserId = ou1.OrganizationUserEntityId,
                        FirstName = ou1.FirstName,
                        LastName = ou1.LastName,
                        UserType = ou1.UserType,
                        IsActive = ou1.IsActive,
                        NumEntities = outer.Count()
                    })
                .Where(so => so.NumEntities == 1)
                .ToList();

            return new GetSingleOrganizationOrgUsersResult
            {
                Result = singleEntityUsers
            };
        }

        public QueryOrganizationUserResult QueryOrganizationUser(QueryOrganizationUserCriteria model)
        {
            //var orgUsers = context.OrganizationUsers;
            IQueryable<OrganizationUserEntity> queryable = this
                    .GetBaseOrganizationUserEntity()
                    .Include(t => t.Profile)
                    .ThenInclude(p => p.ProfileValues)
                    .ThenInclude(pv => pv.ProfileParameter)
                    .Where(t => t.DeletedAt == null && t.RollbackedAt == null)
                ;

            if (model.OrganizationIds != null && model.OrganizationIds.Count > 0)
            {
                queryable = queryable
                        .Where(e => model.OrganizationIds.Contains(e.OrganizationId))
                        .Include(t => t.Profile)
                        .ThenInclude(p => p.ProfileValues)
                        .ThenInclude(pv => pv.ProfileParameter)
                    ;
            }

            if (model.UserType >= 1)
            {
                queryable = queryable
                        .Where(e => e.UserType == model.UserType)
                        .Include(t => t.Profile)
                        .ThenInclude(p => p.ProfileValues)
                        .ThenInclude(pv => pv.ProfileParameter)
                    ;
            }

            var queryOrganizationUserResult = new QueryOrganizationUserResult
            {
                OrganizationUsersData = queryable.Select(p => this.mapper.Map<OrganizationUserListModel>(p)).ToList(),
                Total = queryable.Count()
            };

            return queryOrganizationUserResult;
        }

        public QueryOrganizationUsersResult QueryOrganizationUsers(QueryOrganizationUsersCriteria criteria)
        {
            if (criteria.Page == null || criteria.PageSize == null)
                return null;

            var pageSize = criteria.PageSize.Value;
            var pageValue = criteria.Page.Value;

            var queryable = this
                .GetValidOrganizationUsers(criteria.OrganizationId);

            if (criteria.UserTypes.Any())
                queryable = queryable.Where(ou => criteria.UserTypes.Contains(ou.UserType));

            if (!string.IsNullOrWhiteSpace(criteria.Q))
            {
                queryable = queryable.Where(
                    p => p.FirstName.Contains(criteria.Q, StringComparison.OrdinalIgnoreCase) ||
                         p.LastName.Contains(criteria.Q, StringComparison.OrdinalIgnoreCase) ||
                         p.Email != null && p.Email.Contains(criteria.Q, StringComparison.OrdinalIgnoreCase)
                );
            }

            if (criteria.IsActive.HasValue)
            {
                var isActive = (bool)criteria.IsActive;
                queryable = queryable.Where(p => p.IsActive == isActive);
            }

            if (!string.IsNullOrWhiteSpace(criteria.OrderBy))
            {
                bool orderByAscending = criteria.Direction.ToUpper() == "ASC";
                var orderByField = criteria.OrderBy.ToLowerInvariant();

                switch (orderByField)
                {
                    case "firstname":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => p.FirstName)
                            : queryable.OrderByDescending(p => p.FirstName);
                        break;
                    case "lastname":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => p.LastName)
                            : queryable.OrderByDescending(p => p.LastName);
                        break;
                    case "email":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => p.Email)
                            : queryable.OrderByDescending(p => p.Email);
                        break;
                    case "username":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => p.Username)
                            : queryable.OrderByDescending(p => p.Username);
                        break;
                    case "createdat":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => p.CreatedAt)
                            : queryable.OrderByDescending(p => p.CreatedAt);
                        break;
                    case "islocked":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => !p.IsLocked)
                            : queryable.OrderByDescending(p => !p.IsLocked);
                        break;
                    case "ispendingactivation":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => !p.IsPendingActivation)
                            : queryable.OrderByDescending(p => !p.IsPendingActivation);
                        break;
                    case "isactive":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => !p.IsActive)
                            : queryable.OrderByDescending(p => !p.IsActive);
                        break;
                    case "lastlogintime":
                        queryable = orderByAscending
                            ? queryable.OrderBy(p => p.LastLoginAt)
                            : queryable.OrderByDescending(p => p.LastLoginAt);
                        break;
                }
            }

            //var resultQuery = queryable
            //    .Join(context.OrganizationUserRoles,
            //        organizationUser => organizationUser.OrganizationUserEntityId,
            //        organizationUserRoles => organizationUserRoles.OrganizationUserRoleEntityId,
            //        (organizationUser, organizationUserRoles) => new { organizationUser, organizationUserRoles })

            //    .Join(context.RolePermissions,
            //        rolePermission => rolePermission.organizationUserRoles.RoleId,
            //        r => r.RoleId,
            //        (rolePermission, r) => new { rolePermission, r }
            //        )                
            //    ;

            var count = queryable.Count();

            int page = (pageValue - 1) * pageSize;

            var views = queryable
                .Skip(page)
                .Take(pageSize)
                .ToList();

            var results = views.Select(p => this.mapper.Map<OrganizationUsersListModel>(p))
                .ToList();

            foreach (var x in results)
            {
                var roles = this
                    .GetValidOrganizationUserRoles(x.OrganizationUserId, x.OrganizationId)
                    .Join(context.RolePermissions,
                        organizationUserRoles => organizationUserRoles.RoleId,
                        rolePermissions => rolePermissions.RoleId,
                        (our, rp) => new OrganizationUserRoleCustomModel
                        {
                            RoleName = rp.RoleName,
                            Description = rp.Description,
                            IsDefault = rp.IsDefault,
                            DeletedAt = rp.DeletedAt,
                            DisplayName =rp.DisplayName
                            
                        }
                    )
                    .Where(ourcm => ourcm.DeletedAt == null);

                x.Roles = roles.Select(
                        p => this.mapper.Map<RoleListModel>(p)
                        ).ToList();
            }


            var result = new QueryOrganizationUsersResult
            {
                Pagination = new PaginationModel(count, pageValue, pageSize),
                Result = results
            };

            return result;
        }

        public List<int> QueryUserOrganizations(string userId)
        {
            var sqlQuery = @"
                SELECT ou.OrganizationId
                FROM 
                    OrganizationUser ou left join UserInvitation ui on ou.OrganizationUserEntityId = ui.OrganizationUserId
                WHERE ou.UserId = @userId
                AND ui.DeletedAt is null
                AND ui.DeclinedAt is null
                AND (ui.AcceptedAt is not null OR ui.UserInvitationEntityId is null)
            ";
            
            
            return context
                .OrganizationUsers
                .FromSql(
                    sqlQuery,
                    new SqlParameter("@userId", userId)
                    )
                .AsNoTracking()
                .Select(p => p.OrganizationId)
                .ToList();
        }

        public void RollbackCreateOrganizationUser(string transactionId)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                var organizationUsers = GetOrganizationUsersByTransactionId(transactionId);
                if (organizationUsers.Count > 0)
                {
                    var userRollbackedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    foreach (var user in organizationUsers)
                    {
                        user.RollbackedAt = userRollbackedAt;
                        if (user.AssociatedOrganizationUsers?.Count > 0)
                        {
                            foreach (var associatedOrganizationUser in user.AssociatedOrganizationUsers)
                            {
                                associatedOrganizationUser.RollbackedAt = userRollbackedAt;
                            }
                        }
                    }

                    this.context.SaveChanges();
                    transaction.Commit();
                }
            }
        }

        public bool IsEmailInUse(string email, int? organizationUserId)
        {
            return context
                .OrganizationUsers
                .Any(e =>
                    string.Equals(e.Email, email, StringComparison.CurrentCultureIgnoreCase) &&
                    // since it's conditional, this won't break the query
                    (!organizationUserId.HasValue || e.OrganizationUserEntityId != organizationUserId.Value) &&
                    e.DeletedAt == null &&
                    e.RollbackedAt == null
                );
        }

        public GetOrganizationUserResult SetOrganizationUserLockedStatus(string externalUserId, bool isLocked)
        {
            var users = GetBaseOrganizationUserEntity()
                .Where(ou => string.Equals(ou.UserId, externalUserId, StringComparison.OrdinalIgnoreCase))
                .ToList();


            users.ForEach(ou => ou.IsLocked = isLocked);
            context.UpdateRange(users);
            context.SaveChanges();

            var first = users.First();
            return this.mapper.Map<GetOrganizationUserResult>(first);
        }

        public GetOrganizationUserResult SetUserPendingActivationStatus(string externalUserId, bool isPendingActivation)
        {
            var users = GetBaseOrganizationUserEntity()
                .Where(ou => string.Equals(ou.UserId, externalUserId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            
            users.ForEach(ou => ou.IsPendingActivation = isPendingActivation);
            context.UpdateRange(users);
            context.SaveChanges();
            
            var first = users.First();
            return this.mapper.Map<GetOrganizationUserResult>(first);
        }
        
        public GetOrganizationUserResult SetLastLoginTime(string externalUserId, long loginTimestamp)
        {
            var users = GetBaseOrganizationUserEntity()
                .Where(ou => string.Equals(ou.UserId, externalUserId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            
            users.ForEach(ou => ou.LastLoginAt = loginTimestamp);
            context.UpdateRange(users);
            context.SaveChanges();
            
            var first = users.First();
            return this.mapper.Map<GetOrganizationUserResult>(first);
        }

        #endregion

        #region public API: Associations
        public bool AssociationExists(CreateOrganizationUsersAssociationCommand associationCommand) =>
            this
                .context
                .AssociatedOrganizationUserEntities
                .Any(d => d.AssociationType == associationCommand.AssociationType &&
                          d.OrganizationUserId1 == associationCommand.OrganizationUserId1 &&
                          d.OrganizationUserId2 == associationCommand.OrganizationUserId2 &&
                          d.DeletedAt == null);


        public bool AssociationTypeExists(int organizationUserId, int associationType) =>
            context
                .AssociatedOrganizationUserEntities
                .Any(d => d.AssociationType == associationType &&
                          d.OrganizationUserId1 == organizationUserId &&
                          d.DeletedAt == null);


        /// <inheritdoc />
        public int CreateAssociationOrganizationUser(CreateOrganizationUsersAssociationCommand associationCommand)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var entity = mapper.Map<AssociatedOrganizationUserEntity>(associationCommand);
            //TODO: add CreatedBy details for traceability
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
            this.context.AssociatedOrganizationUserEntities.Add(entity);
            context.SaveChanges();
            return entity.AssociatedOrganizationUserEntityId;
        }

        /// <inheritdoc />
        public int DeleteAssociationOrganizationUser(int organizationUserId, int associationType)
        {
            var entity = GetAssociatedOrganizationUser(organizationUserId, associationType);
            //TODO: add DeletedBy details for traceability
            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            context.SaveChanges();
            return entity.AssociatedOrganizationUserEntityId;
        }

        public void UnDeleteAssociationOrganizationUser(CreateOrganizationUsersAssociationCommand associationCommand, int associatedOrganizationUserEntityId)
        {
            var entityToUpdate = this.context.AssociatedOrganizationUserEntities.First(aoue => aoue.AssociatedOrganizationUserEntityId == associatedOrganizationUserEntityId);
            //TODO: add UpdatedBy details for traceability
            entityToUpdate.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entityToUpdate.DeletedBy = null;
            entityToUpdate.DeletedAt = null;

            this.context.SaveChanges();
        }

        #endregion

        #region internal API   

        private IQueryable<OrganizationUserEntity> GetValidOrganizationUsers(int organizationId)
        {
            return this
                .context
                .OrganizationUsers
                .Where(p => p.OrganizationId == organizationId &&
                            p.RollbackedAt == null &&
                            p.DeletedAt == null
                );
        }

        private List<OrganizationUserEntity> GetOrganizationUsersByTransactionId(string transactionId) =>
            this.GetBaseOrganizationUserEntity()
                .Where(t => t.TransactionId == transactionId)
                .Include(u => u.AssociatedOrganizationUsers)
                .ToList();

        private OrganizationUserEntity GetOrganizationUserEntity(int organizationUserId, int userType, int organizationId) =>
            this.GetBaseOrganizationUserEntity()
                .Include(t => t.Profile)
                .ThenInclude(p => p.ProfileValues)
                .ThenInclude(pv => pv.ProfileParameter)
                .Include(t => t.AssociatedOrganizationUsers)
                .Include(t => t.Groups)
                .ThenInclude(t => t.Group)
                .Include(t => t.UserInvitation)
                .FirstOrDefault(p => p.OrganizationUserEntityId == organizationUserId && p.OrganizationId == organizationId && p.UserType == userType);

        private OrganizationUserEntity GetOrganizationUserEntity(int organizationUserId, int organizationId) =>
            this.GetBaseOrganizationUserEntity()
                .Include(t => t.Profile)
                .ThenInclude(p => p.ProfileValues)
                .ThenInclude(pv => pv.ProfileParameter)
                .Include(t => t.UserInvitation)
                .Include(t => t.AssociatedOrganizationUsers)
                .FirstOrDefault(p => p.OrganizationUserEntityId == organizationUserId &&
                                     p.OrganizationId == organizationId);

        private IQueryable<OrganizationUserEntity> GetBaseOrganizationUserEntity() =>
            this
                .context
                .OrganizationUsers
                .Where(ou => ou.DeletedAt == null && ou.RollbackedAt == null); //TODO should this also be filtered by organizationId?

        private AssociatedOrganizationUserEntity GetAssociatedOrganizationUser(int organizationUserId, int associationType) =>
            this
                .context
                .AssociatedOrganizationUserEntities
                .FirstOrDefault(d => d.AssociationType == associationType &&
                                     d.OrganizationUserId1 == organizationUserId);

        private IQueryable<AssociatedOrganizationUserEntity> GetAllAssociationsWithUser(int organizationUserId)
        {
            return context
                .AssociatedOrganizationUserEntities
                .Where(a => a.DeletedAt == null &&
                            a.RollbackedAt == null &&
                            (a.OrganizationUserId1 == organizationUserId || a.OrganizationUserId2 == organizationUserId));
        }

        private IQueryable<OrganizationUserRoleEntity> GetValidOrganizationUserRoles(int organizationUserId, int organizationId)
        {
            return context
                .OrganizationUserRoles
                .Where(our => our.OrganizationId == organizationId &&
                              our.OrganizationUserId == organizationUserId &&
                              our.DeletedAt == null);
        }

        #endregion
    }
}