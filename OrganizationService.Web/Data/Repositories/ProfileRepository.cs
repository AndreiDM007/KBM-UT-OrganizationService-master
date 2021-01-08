using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        #region members

        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        #endregion

        #region public API

        public ProfileRepository(OrganizationServiceDataContext context, ProfileDataMapper profileDataMapper)
        {
            this.context = context;
            mapper = profileDataMapper.Mapper;
        }

        public GetProfileResult GetProfile(int profileId, int organizationId)
        {
            var result = GetProfileEntity(profileId, organizationId);

            if (result is null)
            {
                return null;
            }

            return mapper.Map<GetProfileResult>(result);
        }

        public int CreateProfile(CreateProfileCommand command)
        {
            var profile = new ProfileEntity
            {
                OrganizationId =  command.OrganizationId,
                OrganizationUserId = command.OrganizationUserId,
                TransactionId = command.TransactionId,
                ProfileValues = mapper.Map<List<ProfileValueEntity>>(command.ProfileValues)
            };


            using (var transaction = this.context.Database.BeginTransaction())
            {
                this.context.Profiles.Add(profile);
                this.context.SaveChanges();
                transaction.Commit();
            }

            return profile.ProfileEntityId;
        }

        public void UpdateProfile(UpdateProfileCommand command)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                //var currentProfile = GetProfileEntity(updateProfile.ProfileId);
                var currentProfile = GetProfileEntityByOrganizationUserId(command.OrganizationUserId, command.OrganizationId);

                //Removes all existing ProfileValues associated with the profile
                context.ProfileValues.RemoveRange(currentProfile.ProfileValues);

                //Creates the new entries from the request                
                var newProfileValues = mapper.Map<List<ProfileValueEntity>>(command.ProfileValues).ToList();
                currentProfile.ProfileValues = newProfileValues;
                currentProfile.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                this.context.SaveChanges();
                transaction.Commit();
            }
        }

        public void DeleteProfile(DeleteProfileCommand command)
        {
            var currentProfile = GetProfileEntityByOrganizationUserId(command.OrganizationUserId.Value, command.OrganizationId.Value);
            currentProfile.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            this.context.SaveChanges();
        }

        public bool ProfileParameterExists(IEnumerable<int> profileParameterCollection) =>
            this.context
                .ProfileParameters
                .Any(p => profileParameterCollection.Contains(p.ProfileParameterEntityId));

        public bool ProfileExists(int profileId) =>
            this.context
                .Profiles
                .Any(p => p.DeletedAt == null && p.RollbackedAt == null && p.ProfileEntityId == profileId);

        public bool ProfileExistsForOrganizationUser(int organizationUserId, int organizationId) =>
            this.context
                .Profiles
                .Any(p => p.DeletedAt == null && p.RollbackedAt == null && p.OrganizationUserId == organizationUserId);

        public void RollbackCreateOrganizationUserProfile(string transactionId)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                //TODO: There should be only one Profile for each CreateOrganizationUserSaga but you never know?

                var profiles = GetProfilesByTransactionId(transactionId);
                if (profiles.Count > 0)
                {
                    var profileValueRollbackedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    foreach (var profile in profiles)
                    {
                        profile.RollbackedAt = profileValueRollbackedAt;
                        foreach (var profilevalue in profile.ProfileValues)
                        {
                            profilevalue.RollbackedAt = profileValueRollbackedAt;
                        }
                    }

                    this.context.SaveChanges();
                    transaction.Commit();
                }
            }
        }
        
        #endregion

        #region internal API       

        private ProfileEntity GetProfileEntity(int profileId, int organizationId) =>
            this.context
                .Profiles
                .Include(e => e.ProfileValues)
                .ThenInclude(e => e.ProfileParameter)
                .FirstOrDefault(p => p.ProfileEntityId == profileId &&
                                     p.OrganizationId == organizationId &&
                                     p.DeletedAt == null &&
                                     p.RollbackedAt == null);

        private ProfileEntity GetProfileEntityByOrganizationUserId(int organizationUserId, int organizationId) =>
            this.context
                .Profiles
                .Include(e => e.ProfileValues)
                .ThenInclude(e => e.ProfileParameter)
                .FirstOrDefault(p => p.OrganizationUserId == organizationUserId &&
                                     p.OrganizationId == organizationId &&
                                     p.DeletedAt == null &&
                                     p.RollbackedAt == null);

        private List<ProfileEntity> GetProfilesByTransactionId(string transactionId) =>
            this.context
                .Profiles
                .Include(e => e.ProfileValues)
                .Where(p => p.TransactionId == transactionId && p.DeletedAt == null && p.RollbackedAt == null)
                .ToList();

       

        #endregion
    }
}