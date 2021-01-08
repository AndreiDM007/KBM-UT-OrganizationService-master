using System.Collections.Generic;
using System.Linq;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;

namespace OrganizationService.TestHarness
{
    public class ProfileServiceTests
    {
        private readonly ProfileService _profileService;

        private static CreateProfileCommand GenerateProfileModelObject()
        {
            return new CreateProfileCommand
            {
                OrganizationUserId = 9002,
                ProfileValues = new List<ProfileValueCreateModel>()
                {
                    new ProfileValueCreateModel {ProfileParameterId = 1, Value = "23"},
                    new ProfileValueCreateModel {ProfileParameterId = 2, Value = "Street"},
                    new ProfileValueCreateModel {ProfileParameterId = 3, Value = "OB N"},
                }
            };
        }

        private static List<IProfileValueModel> GenerateProfileValues()
        {
            return new List<IProfileValueModel>()
            {
                new ProfileValueCreateModel {ProfileParameterId = 1, Value = "45"},
                new ProfileValueCreateModel {ProfileParameterId = 2, Value = "Home"},
                new ProfileValueCreateModel {ProfileParameterId = 3, Value = "AB D"},
            };
        }

        public ProfileServiceTests(ProfileService profileService)
        {
            _profileService = profileService;
        }

        public bool TestAll()
        {
            var createResult = CreateProfile();
            var getResult = GetProfile(createResult);
            var updateResult = UpdateProfile(createResult);
            var deleteResult = DeleteProfile(createResult);

            //if it reached this far without throwing an exception, the CRUD was done
            return true;
        }

        public int? CreateProfile()
        {
            var createResult = _profileService.CreateProfile(GenerateProfileModelObject());

            ValidateSuccess(createResult, nameof(CreateProfile));

            return createResult.Value;
        }

        public int GetProfile(int? profileId)
        {
            profileId = profileId ?? CreateProfile();
            if (profileId is null)
            {
                throw new ProfileServiceTestException("Unable to execute GetProfile test as there was no profileid or the system was unable to generate one");
            }

            var getResult = _profileService.GetProfile(profileId.Value);
            if (!getResult.IsSuccess)
            {
                var errorDetails = string.Join(", ", getResult.Errors.Select(e => e.Code));
                throw new ProfileServiceTestException($"Failed to {nameof(GetProfile)}. Reason: {errorDetails}");
            }
            return getResult.Value.ProfileId;
        }

        public bool UpdateProfile(int? createResult)
        {
            var updateResult = _profileService.UpdateProfile(new UpdateProfileCommand()
            {
                ProfileId = createResult.Value,
                ProfileValues = GenerateProfileValues()
            });
            ValidateSuccess(updateResult, nameof(UpdateProfile));

            return updateResult.IsSuccess;
        }

        public bool DeleteProfile(int? createResult)
        {
            var deleteResult = _profileService.DeleteProfile(createResult.Value);

            ValidateSuccess(deleteResult, nameof(DeleteProfile));

            return deleteResult.IsSuccess;
        }

        private static void ValidateSuccess(Result<int> createResult, string createProfileName)
        {
            if (!createResult.IsSuccess)
            {
                var errorDetails = string.Join(", ", createResult.Errors.Select(e => e.Code));
                throw new ProfileServiceTestException($"Failed to {createProfileName}. Reason: {errorDetails}");
            }
        }

        private void ValidateSuccess(EmptyResult updateResult, string updateProfileName)
        {
            if (!updateResult.IsSuccess)
            {
                var errorDetails = string.Join(", ", updateResult.Errors.Select(e => e.Code));
                throw new ProfileServiceTestException($"Failed to {updateProfileName}. Reason: {errorDetails}");
            }
        }




    }
}
