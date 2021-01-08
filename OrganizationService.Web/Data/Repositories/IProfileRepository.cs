using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public interface IProfileRepository
    {
        GetProfileResult GetProfile(int profileId, int organizationId);
        int CreateProfile(CreateProfileCommand command);
        void UpdateProfile(UpdateProfileCommand command);
        void DeleteProfile(DeleteProfileCommand command);
        bool ProfileParameterExists(IEnumerable<int> profileParameterCollection);
        bool ProfileExists(int profileId);        
        bool ProfileExistsForOrganizationUser(int organizationUserId, int organizationId);
        void RollbackCreateOrganizationUserProfile(string transactionId);
    }
}