using Kebormed.Core.OrganizationService.Web.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group
{
    public class GroupServiceErrors
    {
        public static InvalidCreateGroupDataError InvalidCreateGroupDataError(string fieldName = null) => new InvalidCreateGroupDataError(fieldName);
        public static InvalidUpdateGroupDataError InvalidUpdateGroupDataError(string fieldName = null) => new InvalidUpdateGroupDataError(fieldName);
        public static InvalidDeleteGroupDataError InvalidDeleteGroupDataError(string fieldName = null) => new InvalidDeleteGroupDataError(fieldName);
        public static InvalidOrganizationIdError InvalidOrganizationIdError() => new InvalidOrganizationIdError();
        public static InvalidGroupNameError InvalidGroupNameError() => new InvalidGroupNameError();
        public static InvalidAssociateOrganizationUserToGroupDataError InvalidAssociateOrganizationUserToGroupDataError(string fieldName = null) => new InvalidAssociateOrganizationUserToGroupDataError(fieldName);
        public static InvalidDisassociateOrganizationUserFromGroupDataError InvalidDisassociateOrganizationUserFromGroupDataError(string fieldName = null) => new InvalidDisassociateOrganizationUserFromGroupDataError(fieldName);
        public static InvalidDisassociateOrganizationUserFromGroupDataError InvalidDisassociateOrganizationUserFromGroupDataError() => new InvalidDisassociateOrganizationUserFromGroupDataError();
        public static GroupAlreadyExistsError GroupAlreadyExistsError() => new GroupAlreadyExistsError();        
        public static GroupNotFoundError GroupNotFoundError() => new GroupNotFoundError();
        public static OrganizationUserNotFoundError OrganizationUserNotFoundError() => new OrganizationUserNotFoundError();
        public static AssociationAlreadyExistsError AssociationAlreadyExistsError() => new AssociationAlreadyExistsError();       
        public static NotFoundError NotFoundError() => new NotFoundError();
        public static InvalidQueryGroupMemberDataError InvalidQueryGroupMemberDataError(string field) => new InvalidQueryGroupMemberDataError(field);        
        public static InvalidQueryGroupsDataError InvalidQueryGroupsDataError(string field) => new InvalidQueryGroupsDataError(field);
        public static InvalidQueryMemberGroupDataError InvalidQueryMemberGroupDataError(string field) => new InvalidQueryMemberGroupDataError(field);
    }
}
