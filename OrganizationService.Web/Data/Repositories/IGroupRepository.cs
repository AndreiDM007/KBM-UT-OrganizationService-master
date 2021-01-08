using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IGroupRepository
    {
        int CreateGroup(CreateGroupCommand command);
        void UpdateGroup(UpdateGroupCommand command);
        void DeleteGroup(DeleteGroupCommand command);
        void AssociateOrganizationUserToGroup(AssociateOrganizationUserToGroupCommand command);
        bool DisassociateOrganizationUserFromGroup(DisassociateOrganizationUserFromGroupCommand command);
        void DisassociateOrganizationUserFromAllGroups(DisassociateOrganizationUserFromAllGroupsCommand command);
        bool GroupExists(string groupName, int organizationId);
        bool GroupExists(int groupId, int organizationId);
        bool GroupNameExists(int groupId, string groupName, int organizationId);
        bool OrganizationUserAssociationWithGroupExists(int groupId, int organizationUserId, int organizationId);
        QueryGroupMemberResult QueryGroupMember(QueryGroupMemberCriteria criteria);
        QueryMemberGroupResult QueryMemberGroup(QueryMemberGroupCriteria criteria);
        QueryGroupResult QueryGroups(int organizationId, bool sortByCreatedAt);
        string[] GetGroupMemberSortableFields();
        string GetDefaultSortField();
        bool ExistGroupByName(string name, int organizationId);
    }
}
