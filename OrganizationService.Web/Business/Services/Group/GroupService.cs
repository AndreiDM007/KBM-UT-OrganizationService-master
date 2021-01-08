using System;
using System.Linq;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;

using Kebormed.Core.OrganizationService.Web.Data.Repositories;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group
{
    public class GroupService
    {
        private readonly IGroupRepository groupRepository;
        private readonly IOrganizationUserRepository organizationUserRepository;

        public GroupService(IGroupRepository groupRepository, IOrganizationUserRepository organizationUserRepository)
        {
            this.groupRepository = groupRepository;
            this.organizationUserRepository = organizationUserRepository;
        }

        public Result<int> CreateGroup(CreateGroupCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                return new Result<int>(GroupServiceErrors.InvalidCreateGroupDataError(nameof(command.OrganizationId)));
            }

            if (command.ParentGroupId <= 0)
            {
                return new Result<int>(GroupServiceErrors.InvalidCreateGroupDataError(nameof(command.ParentGroupId)));
            }

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                return new Result<int>(GroupServiceErrors.InvalidCreateGroupDataError(nameof(command.Name)));
            }

            if (groupRepository.GroupExists(command.Name, command.OrganizationId))
            {
                return new Result<int>(GroupServiceErrors.GroupAlreadyExistsError());
            }        

            return new Result<int>(this.groupRepository.CreateGroup(command));
        }

        public Result<EmptyResult> UpdateGroup(UpdateGroupCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidUpdateGroupDataError(nameof(command.OrganizationId)));
            }

            if (command.GroupId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidUpdateGroupDataError(nameof(command.GroupId)));
            }

            if (!groupRepository.GroupExists(command.GroupId, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.GroupNotFoundError());
            }

            if (groupRepository.GroupNameExists(command.GroupId, command.Name, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.GroupAlreadyExistsError());
            }

            groupRepository.UpdateGroup(command);

            return new Result<EmptyResult>();
        }

        public Result<EmptyResult> DeleteGroup(DeleteGroupCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidDeleteGroupDataError(nameof(command.OrganizationId)));
            }

            if (command.GroupId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidDeleteGroupDataError(nameof(command.GroupId)));
            }

            if (!groupRepository.GroupExists(command.GroupId, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.GroupNotFoundError());
            }            

            groupRepository.DeleteGroup(command);

            return new Result<EmptyResult>();
        }

        public Result<EmptyResult> AssociateOrganizationUserToGroup(AssociateOrganizationUserToGroupCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidAssociateOrganizationUserToGroupDataError(nameof(command.OrganizationId)));
            }

            if (command.GroupId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidAssociateOrganizationUserToGroupDataError(nameof(command.GroupId)));
            }

            if (!groupRepository.GroupExists(command.GroupId, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.GroupNotFoundError());
            }

            if (groupRepository.OrganizationUserAssociationWithGroupExists(command.GroupId, command.OrganizationUserId, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.AssociationAlreadyExistsError());
            }

            // want to check allowed user types to prevent non authorized user-types being added to a group
            var orgUserType = organizationUserRepository.GetOrganizationUserType(command.OrganizationUserId, command.OrganizationId);
            if (!orgUserType.HasValue || !command.AllowedUserTypes.Contains(orgUserType.Value))
            {
                return new Result<EmptyResult>(GroupServiceErrors.OrganizationUserNotFoundError());
            }

            groupRepository.AssociateOrganizationUserToGroup(command);

            return new Result<EmptyResult>();
        }

        public Result<EmptyResult> DisassociateOrganizationUserFromGroup(DisassociateOrganizationUserFromGroupCommand command)
        {
            if (command.OrganizationId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidDisassociateOrganizationUserFromGroupDataError(nameof(command.OrganizationId)));
            }

            if (command.GroupId <= 0)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidDisassociateOrganizationUserFromGroupDataError(nameof(command.GroupId)));
            }

            if (!groupRepository.GroupExists(command.GroupId, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.GroupNotFoundError());
            }

            if (!organizationUserRepository.OrganizationUserExists(command.OrganizationUserId, command.OrganizationId))
            {
                return new Result<EmptyResult>(GroupServiceErrors.OrganizationUserNotFoundError());
            }

            var result = groupRepository.DisassociateOrganizationUserFromGroup(command);
            if (!result)
            {
                return new Result<EmptyResult>(GroupServiceErrors.InvalidDisassociateOrganizationUserFromGroupDataError());
            }            

            return new Result<EmptyResult>();
        }

        public Result<QueryGroupMemberResult> QueryGroupMember(QueryGroupMemberCriteria criteria)
        {
            if (criteria.GroupId <= 0)
            {
                return new Result<QueryGroupMemberResult>(GroupServiceErrors.InvalidQueryGroupMemberDataError(nameof(criteria.GroupId)));
            }

            if (criteria.OrganizationId <= 0)
            {
                return new Result<QueryGroupMemberResult>(GroupServiceErrors.InvalidQueryGroupMemberDataError(nameof(criteria.OrganizationId)));
            }
            if (!groupRepository.GroupExists(criteria.GroupId, criteria.OrganizationId))
            {
                return new Result<QueryGroupMemberResult>(GroupServiceErrors.GroupNotFoundError());
            }
            if (criteria.Page == null || criteria.Page.Value <= 0)
            {
                criteria.Page = 1;
            }

            if (criteria.PageSize == null || criteria.PageSize.Value <= 0)
            {
                criteria.PageSize = 10;
            }

            string[] sortableFields = groupRepository.GetGroupMemberSortableFields();
            if (!sortableFields.Contains(criteria.OrderBy, StringComparer.OrdinalIgnoreCase))
            {
                criteria.OrderBy = groupRepository.GetDefaultSortField();
            }

            if (!string.Equals(criteria.Direction, "DESC", StringComparison.OrdinalIgnoreCase))
            {
                criteria.Direction = "ASC";
            }
            var queryGroupMemberResult = groupRepository.QueryGroupMember(criteria);
            
            return new Result<QueryGroupMemberResult>(queryGroupMemberResult);
        }

        public Result<QueryMemberGroupResult> QueryMemberGroup(QueryMemberGroupCriteria criteria)
        {
            if (criteria.MemberId <= 0)
            {
                return new Result<QueryMemberGroupResult>(GroupServiceErrors.InvalidQueryMemberGroupDataError(nameof(criteria.MemberId)));
            }

            if (criteria.OrganizationId <= 0)
            {
                return new Result<QueryMemberGroupResult>(GroupServiceErrors.InvalidQueryMemberGroupDataError(nameof(criteria.OrganizationId)));
            }                        
            var result = groupRepository.QueryMemberGroup(criteria);
            
            return new Result<QueryMemberGroupResult>(result);
        }
        
        public Result<QueryGroupResult> QueryGroups(int organizationId)
        {
            if (organizationId <= 0)
            {
                return new Result<QueryGroupResult>(GroupServiceErrors.InvalidQueryGroupsDataError(nameof(organizationId)));
            }
           
            var result = groupRepository.QueryGroups(organizationId, true);

            return new Result<QueryGroupResult>(result);
        }

        public Result<bool> ExistGroupByName(string name, int organizationId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new Result<bool>(GroupServiceErrors.InvalidGroupNameError());
            }

            if (organizationId <= 0)
            {
                return new Result<bool>(GroupServiceErrors.InvalidOrganizationIdError());
            }

            var result = groupRepository.ExistGroupByName(name, organizationId);
            return new Result<bool>(result);
        }
    }
}
