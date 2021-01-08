using System.Collections.Generic;
using System.Linq;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Profile
{
    public class ProfileService
    {
        private readonly IProfileRepository profileRepository;
        private readonly IOrganizationUserRepository organizationUserRepository;

        public ProfileService(IProfileRepository profileRepository, IOrganizationUserRepository organizationUserRepository)
        {
            this.profileRepository = profileRepository;
            this.organizationUserRepository = organizationUserRepository;
        }

        public Result<GetProfileResult> GetProfile(int profileId, int organizationId)
        {
            var profileResult = profileRepository.GetProfile(profileId, organizationId);

            if (profileResult is null)
            {
                return new Result<GetProfileResult>(new InvalidProfileIdError());
            }

            return new Result<GetProfileResult>(profileResult);
        }

        public Result<int> CreateProfile(CreateProfileCommand command)
        {
            if (profileRepository.ProfileExistsForOrganizationUser(command.OrganizationUserId, command.OrganizationId))
            {
                return new Result<int>(ProfileServiceErrors.ProfileForOrganizationUserAlreadyExists());
            }

            // TODO -  think about the wanted strategy for empty/optional profile values
            /*if (ValidateProfileValue(command.ProfileValues))
            {
                return new Result<int>(ProfileServiceErrors.InvalidProfileValuesData());
            }*/

            if (!organizationUserRepository.OrganizationUserExists(command.OrganizationUserId, command.OrganizationId))
            {
                return new Result<int>(ProfileServiceErrors.InvalidOrganizationUserId());
            }

            var (invalidProfileParametersFound, invalidProfileParametersCollection) = ValidateProfileParameters(command.ProfileValues.ToList<IProfileValueModel>());

            if (invalidProfileParametersFound)
            {
                return new Result<int>(invalidProfileParametersCollection);
            }

            var profileId = profileRepository.CreateProfile(command);
            return new Result<int>(profileId);
        }

        public EmptyResult UpdateProfile(UpdateProfileCommand command)
        {            
            if (!profileRepository.ProfileExistsForOrganizationUser(command.OrganizationUserId, command.OrganizationId))
            {
                return new Result<int>(ProfileServiceErrors.InvalidOrganizationUserId());
            }

            var (invalidProfileParametersFound, invalidProfileParametersCollection) = ValidateProfileParameters(command.ProfileValues.ToList<IProfileValueModel>());

            if (invalidProfileParametersFound)
            {
                return new Result<int>(invalidProfileParametersCollection);
            }

            profileRepository.UpdateProfile(command);
            return new EmptyResult();
        }

        public EmptyResult DeleteProfile(DeleteProfileCommand command)
        {            
            if (!command.OrganizationUserId.HasValue)
            {
                return new Result<EmptyResult>(ProfileServiceErrors.InvalidOrganizationUserId());
            }         
            if (!command.OrganizationId.HasValue)
            {
                return new Result<EmptyResult>(ProfileServiceErrors.InvalidOrganizationId());
            }
            
            if (!profileRepository.ProfileExistsForOrganizationUser(command.OrganizationUserId.Value, command.OrganizationId.Value))
            {
                return new EmptyResult(ProfileServiceErrors.InvalidProfileId());
            }

            profileRepository.DeleteProfile(command);
            return new EmptyResult();
        }

        private (bool invalidFound, IReadOnlyCollection<InvalidProfileParameterIdError> invalid) ValidateProfileParameters(IReadOnlyCollection<IProfileValueModel> model)
        {
            var invalid = new List<InvalidProfileParameterIdError>();

            // TODO - avoid doing so many queries to the DB like this in a for loops or similar 
            foreach (var profileValue in model)
            {
                if (!profileRepository.ProfileParameterExists(model.Select(t => t.ProfileParameterId)))
                {
                    invalid.Add(ProfileServiceErrors.InvalidProfileParameterId(profileValue.ProfileParameterId));
                }
            }

            return (invalid.Any(), invalid);
        }

        private bool ValidateProfileValue(IReadOnlyCollection<ProfileValueCreateModel> profileValues) =>
            profileValues.Any(pv => string.IsNullOrWhiteSpace(pv.Value));
    }
}