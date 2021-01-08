using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        #region Members

        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        const string GmSortUsername = "username";
        const string GmSortFirstName = "firstname";
        const string GmSortLastName = "lastname";
        const string GmSortCreationtime = "creationtime";
        const string GmSortUserType = "usertype";

        public static readonly string[] __GroupMemberSortableFields =
        {
            GmSortUsername,
            GmSortCreationtime,
            GmSortUserType,
            GmSortFirstName,
            GmSortLastName
        };

        #endregion

        public GroupRepository(OrganizationServiceDataContext context, GroupDataMapper mapper)
        {
            this.context = context;
            this.mapper = mapper.Mapper;
        }

        public bool GroupExists(string groupName, int organizationId) =>
            GetValidGroups(organizationId).Any(g => g.Name == groupName);

        public bool GroupExists(int groupId, int organizationId) =>
            GetValidGroups(organizationId).Any(g => g.GroupEntityId == groupId);

        public bool GroupNameExists(int groupId, string groupName, int organizationId) =>
            GetValidGroups(organizationId).Any(g => g.GroupEntityId != groupId && g.Name == groupName);

        public bool OrganizationUserAssociationWithGroupExists(int groupId, int organizationUserId, int organizationId) =>
            GetValidGroupMembers(organizationId).Any(gma => gma.GroupId == groupId && gma.OrganizationUserId == organizationUserId);
       
        public int CreateGroup(CreateGroupCommand command)
        {
            var entity = mapper.Map<GroupEntity>(command);
            entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.Groups.Add(entity);
            this.context.SaveChanges();

            return entity.GroupEntityId;
        }

        public void UpdateGroup(UpdateGroupCommand command)
        {
            var entity = this
                .GetValidGroups(command.OrganizationId)
                .FirstOrDefault(g => g.GroupEntityId.Equals(command.GroupId));

            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entity.UpdatedAt = now;

            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                entity.Name = command.Name;
            }

            if (!string.IsNullOrWhiteSpace(command.Description))
            {
                entity.Description = command.Description;
            }

            if (command.ParentGroupId > 0 || command.ParentGroupId == null)
            {
                entity.ParentGroupId = command.ParentGroupId;
            }

            context.Groups.Update(entity);
            this.context.SaveChanges();
        }

        public void DeleteGroup(DeleteGroupCommand command)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            CascadeDeleteGroup(command.GroupId, command.OrganizationId, now);
            context.SaveChanges();
        }

        public void AssociateOrganizationUserToGroup(AssociateOrganizationUserToGroupCommand command)
        {
            var entity = mapper.Map<GroupMemberEntity>(command);
            entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.GroupMembers.Add(entity);
            this.context.SaveChanges();
        }

        public bool DisassociateOrganizationUserFromGroup(DisassociateOrganizationUserFromGroupCommand command)
        {
            var entity = this
                .GetValidGroupMembers(command.OrganizationId)
                .FirstOrDefault(g => g.GroupId.Equals(command.GroupId) &&
                                     g.OrganizationUserId.Equals(command.OrganizationUserId));

            if (entity is null)
            {
                return false;
            }

            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();            
            context.SaveChanges();

            return true;
        }

        public void DisassociateOrganizationUserFromAllGroups(DisassociateOrganizationUserFromAllGroupsCommand command)
        {
            var entities = this
                .GetValidGroupMembers(command.OrganizationId)
                .Where(gm => gm.OrganizationUserId == command.OrganizationUserId);

            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            foreach (var entity in entities)
            {
                entity.DeletedAt = now;
            }

            context.SaveChanges();
        }

        public string[] GetGroupMemberSortableFields()
        {
            return __GroupMemberSortableFields;
        }

        public string GetDefaultSortField()
        {
            return GmSortCreationtime;
        }

        public QueryGroupMemberResult QueryGroupMember(QueryGroupMemberCriteria criteria)
        {
            if (criteria.Page == null || criteria.PageSize == null)
                return null;

            var pageSize = criteria.PageSize.Value;
            var pageValue = criteria.Page.Value;

            var targetUserTypes = criteria.TargetUserTypes;

            var query = GetValidGroupMembers(criteria.OrganizationId)
                .Include(gm => gm.Member)
                .Where(gm => gm.GroupId == criteria.GroupId &&
                             targetUserTypes.Any(t => t == gm.Member.UserType) &&
                             gm.Member.DeletedAt == null); 

            if (!string.IsNullOrWhiteSpace(criteria.OrderBy))
            {
                bool orderByAscending = criteria.Direction.ToUpper() == "ASC";
                var orderByField = criteria.OrderBy.ToLowerInvariant();

                switch (orderByField)
                {
                    case GmSortUsername:
                        query = query.OrderBy(p => p.Member.Username, !orderByAscending);
                        break;
                    case GmSortCreationtime:
                        query = query.OrderBy(p => p.CreatedAt, !orderByAscending);
                        break;
                    case GmSortUserType:
                        query = query.OrderBy(p => p.Member.UserType, !orderByAscending);    
                        break;
                    case GmSortFirstName:
                        query = query.OrderBy(p => p.Member.FirstName, !orderByAscending);
                        break;
                    case GmSortLastName:
                        query = query.OrderBy(p => p.Member.LastName, !orderByAscending);
                        break;
                    default:
                        query = query.OrderBy(p => p.CreatedAt, true);
                        break;
                }
            }
            var count = query.Count();

            int page = (pageValue - 1) * pageSize;

            var views = query
                .Skip(page)
                .Take(pageSize)
                .ToList();

            var results = views
                .Select(p => this.mapper.Map<GroupMemberListModel>(p))
                .ToList();

            var result = new QueryGroupMemberResult
            {
                Pagination = new PaginationModel(count, pageValue, pageSize),
                Result = results
            };

            return result;
        }

        public QueryMemberGroupResult QueryMemberGroup(QueryMemberGroupCriteria criteria)
        {
            if (criteria == null)
                return null;
            
            var query = GetValidGroupMembers(criteria.OrganizationId)
                .Include(gm => gm.Group)
                .Where(gm => gm.OrganizationUserId == criteria.MemberId);
                                 
            var views = query                
                .ToList();

            var results = views
                .Select(p => this.mapper.Map<MemberGroupListModel>(p))
                .ToList();

            var result = new QueryMemberGroupResult
            {                
                Result = results
            };

            return result;
        }

        public QueryGroupResult QueryGroups(int organizationId, bool sortByCreatedAt)
        {
            var query = GetValidGroups(organizationId);
                
            if(sortByCreatedAt)
                query = query.OrderByDescending(p => p.CreatedAt);

            var results = query.Select(p => this.mapper.Map<GroupListModel>(p)).ToList();

            var result = new QueryGroupResult
            {
                Result = results
            };

            return result;
        }

        public bool ExistGroupByName(string name, int organizationId) =>
            this.GetValidGroups(organizationId)
                .Any(g => g.Name.Equals(name));

        
        private void CascadeDeleteGroup(int groupEntityId, int organizationId, long now)
        {
            var entity = this
                .GetValidGroups(organizationId)
                .Include(g => g.GroupMembers)
                .Include(g => g.SubGroups)
                .FirstOrDefault(g => g.GroupEntityId == groupEntityId);

            //it's not worth to filter members since we'd have to iterate the list twice
            if (entity.GroupMembers != null && entity.GroupMembers.Count > 0)
            {
                foreach (var groupMember in entity.GroupMembers)
                {
                    groupMember.DeletedAt = now;
                }
            }

            if (entity.SubGroups != null && entity.SubGroups.Count > 0)
            {
                foreach (var subGroup in entity.SubGroups)
                {
                    // since we can't filter the includes, subgroups that have been already deleted
                    // have to be filtered out
                    if(subGroup.DeletedAt == null)
                        this.CascadeDeleteGroup(subGroup.GroupEntityId, organizationId, now);
                }
            }

            entity.DeletedAt = now;
        }
        
        private IQueryable<GroupEntity> GetValidGroups(int organizationId)
        {
            return this
                .context
                .Groups
                .Where(g => g.OrganizationId == organizationId &&
                            g.DeletedAt == null);
        }

        private IQueryable<GroupMemberEntity> GetValidGroupMembers(int organizationId)
        {
            return context
                .GroupMembers
                .Where(gm => gm.OrganizationId == organizationId &&
                             gm.DeletedAt == null);
        }       
    }
}
