using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IOrganizationUserRepository
    {
        /// <summary>
        /// Checks if the OrganizationUser Exists based on Primary Key - OrganizationUserId
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool OrganizationUserExists(int organizationUserId, int organizationId);

        /// <summary>
        /// Checks if the Organization User Exists based on the two main Foreign Keys that give origin to this table
        /// UserId, OrganizationId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userType"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool OrganizationUserExists(string userId, int userType, int organizationId);

        /// <summary>
        /// Given an OrganizationUserId and UserType validates if the pair is already created
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="userType"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool OrganizationUserExists(int organizationUserId, int userType, int organizationId);

        /// <summary>
        /// Get organization user by email, userType and organizationId
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userType"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool OrganizationUserExistsByEmail(string email);

        /// <summary>
        /// Get organization user by username, userType, organizationId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userType"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        bool OrganizationUserExistsByUsername(string username);

        int? GetOrganizationUserType(int organizationUserId, int organizationId);
        GetOrganizationUserResult GetOrganizationUser(int organizationUserId, int userType, int organizationId);

        GetOrganizationUserByExternalUserIdResult GetOrganizationUserByExternalUserId(int organizationId, string userId);
        
        GetOrganizationAdminResult GetOrganizationAdmin(int organizationId, int userType);

        int CreateOrganizationUser(CreateOrganizationUserCommand model);
        void UpdateOrganizationUser(UpdateOrganizationUserCommand command);

        long DeleteOrganizationUser(int organizationUserId, int userType, int organizationId);

        AnyOrganizationUserWithSharedUserId AnyOrganizationUserWithSharedUserId(int organizationUserId);

        GetSingleOrganizationOrgUsersResult GetSingleOrganizationOrgUsers(int organizationId);

        QueryOrganizationUserResult QueryOrganizationUser(QueryOrganizationUserCriteria model);
        QueryOrganizationUsersResult QueryOrganizationUsers(QueryOrganizationUsersCriteria criteria);

        List<int> QueryUserOrganizations(string userId);       

        /// <summary>
        /// This will check if the given email is being already used by any user.
        /// </summary>
        /// <param name="email">email to be searched</param>
        /// <param name="organizationUserId">if not null, will query other users other than the given one</param>
        /// <returns>true if in use, false otherwise</returns>
        bool IsEmailInUse(string email, int? organizationUserId);

        GetOrganizationUserResult SetOrganizationUserLockedStatus(string externalUserId, bool isLocked);

        GetOrganizationUserResult SetUserPendingActivationStatus(string externalUserId, bool isPendingActivation);

        GetOrganizationUserResult SetLastLoginTime(string externalUserId, long loginTimestamp);
        
        // ############################ Associations
        
        bool AssociationExists(CreateOrganizationUsersAssociationCommand associationCommand);

        /// <summary>
        /// Creates a new association between Organization Users, with a given type, meaningful to the business
        /// </summary>
        /// <param name="associationCommand"></param>
        /// <returns></returns>
        int CreateAssociationOrganizationUser(CreateOrganizationUsersAssociationCommand associationCommand);

        /// <summary>
        /// Mark a previously existing association as deleted
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="associationType"></param>
        /// <returns></returns>
        int DeleteAssociationOrganizationUser(int organizationUserId, int associationType);

        /// <summary>
        /// Checks if an association of a certain type already exists for a specific organization User 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="associationType"></param>
        /// <returns></returns>
        bool AssociationTypeExists(int organizationUserId, int associationType);

        void RollbackCreateOrganizationUser(string transactionId);
    }
}