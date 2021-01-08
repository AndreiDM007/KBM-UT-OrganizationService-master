using System;
using System.Collections.Generic;
using System.Linq;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;

namespace OrganizationService.TestHarness
{
    public class OrganizationUserServiceTests
    {
        private readonly OrganizationUserService _organizationUserService;

        private static CreateOrganizationUserCommand GenerateOrganizationUserModelObject()
        {
            return new CreateOrganizationUserCommand
            {
                UserId = DateTime.Now.Ticks.ToString(),
                OrganizationId = 1,
                UserType = 1
            };
        }

        public OrganizationUserServiceTests(OrganizationUserService organizationUserService)
        {
            _organizationUserService = organizationUserService;
        }

        public bool TestAll()
        {
            var createResult = CreateOrganizationUser();
            var getResult = GetOrganizationUser(createResult);

            //if it reached this far without throwing an exception, the CRUD was done
            return true;
        }

        public int? CreateOrganizationUser()
        {
            var createResult = _organizationUserService.CreateOrganizationUser(GenerateOrganizationUserModelObject());

            ValidateSuccess(createResult, nameof(CreateOrganizationUser));

            return createResult.Value;
        }

        private static void ValidateSuccess(Result<int> createResult, string createProfileName)
        {
            if (!createResult.IsSuccess)
            {
                var errorDetails = string.Join(", ", createResult.Errors.Select(e => e.Code));
                throw new ProfileServiceTestException($"Failed to {createProfileName}. Reason: {errorDetails}");
            }
        }

        public int GetOrganizationUser(int? OrganizationUserId)
        {
            OrganizationUserId = OrganizationUserId ?? CreateOrganizationUser();
            if (OrganizationUserId is null)
            {
                throw new Exception("Unable to execute GetOrganizationUser test as there was no OrganizationUserid or the system was unable to generate one");
            }

            var getResult = _organizationUserService.GetOrganizationUser(OrganizationUserId.Value);
            if (!getResult.IsSuccess)
            {
                var errorDetails = string.Join(", ", getResult.Errors.Select(e => e.Code));
                throw new Exception($"Failed to {nameof(GetOrganizationUser)}. Reason: {errorDetails}");
            }
            return getResult.Value.OrganizationId;
        }
    }
}