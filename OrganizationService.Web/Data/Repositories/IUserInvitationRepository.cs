using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IUserInvitationRepository
    {
        /// <summary>
        /// Checks if an invitation exists
        /// </summary>
        /// <param name="invitationGuid"></param>
        /// <param name="externalUserId"></param>
        /// <returns></returns>
        bool UserInvitationExists(string invitationGuid, string externalUserId);
        
        /// <summary>
        /// Fetches a given invitation
        /// </summary>
        /// <param name="invitationGuid"></param>
        /// <param name="externalUserId"></param>
        /// <returns></returns>
        GetUserInvitationResult GetUserInvitation(string invitationGuid, string externalUserId);

        /// <summary>
        /// Creates a new user invitation and returns its ID
        /// </summary>
        /// <param name="command">invite details</param>
        /// <param name="inviteGuid">invite guid</param>
        /// <returns>Created invite Id</returns>
        int CreateUserInvitation(CreateUserInvitationCommand command, string inviteGuid);

        /// <summary>
        /// Updates the invitation state
        /// </summary>
        /// <param name="command">Detials of whether the invitation was accepted or declined</param>
        void UpdateUserInvitation(UpdateUserInvitationCommand command);

        /// <summary>
        /// Deletes an invitation
        /// </summary>
        /// <param name="invitationGuid">GUID of invite</param>
        /// <param name="externalUserId"></param>
        void DeleteUserInvitation(string invitationGuid, string externalUserId);

    }
}