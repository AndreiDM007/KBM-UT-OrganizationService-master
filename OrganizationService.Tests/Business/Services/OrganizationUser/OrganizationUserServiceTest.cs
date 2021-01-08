using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace OrganizationService.Tests.Business.Services.OrganizationUser
{
    public class OrganizationUserServiceTest
    {
        private readonly Mock<IOrganizationUserRepository> organizationUserRepository;
        private readonly Mock<IOrganizationRepository> organizationRepository;
        private readonly Mock<IProfileRepository> profileRepository;
        private readonly Mock<IGroupRepository> groupRepository;
        private readonly Mock<IMessagingClient> messagingClient;
        private readonly Mock<ILogger<OrganizationUserService>> organizationUserServiceLogger;
        private readonly Mock<IOrganizationUserRoleRepository> roleRepository;

        private OrganizationUserService organizationUserService;

        public OrganizationUserServiceTest()
        {
            organizationUserRepository = new Mock<IOrganizationUserRepository>();
            organizationRepository = new Mock<IOrganizationRepository>();
            groupRepository = new Mock<IGroupRepository>();
            profileRepository = new Mock<IProfileRepository>();
            messagingClient = new Mock<IMessagingClient>();
            organizationUserServiceLogger = new Mock<ILogger<OrganizationUserService>>();
            roleRepository = new Mock<IOrganizationUserRoleRepository>();

            organizationUserService = new OrganizationUserService(
                organizationUserRepository.Object,
                organizationRepository.Object,
                groupRepository.Object,
                profileRepository.Object,
                roleRepository.Object,
                messagingClient.Object,
                organizationUserServiceLogger.Object);
        }

        [Theory]
        [InlineData("user1@email.com", true)]
        [InlineData("user2@email.com", false)]
        [InlineData("", false, true)]
        public void ExistOrganizationUserByEmail(string email, bool result, bool isError = false)
        {
            organizationUserRepository.Setup(x =>
                    x.OrganizationUserExistsByEmail("user1@email.com"))
                .Returns(true);

            var res = organizationUserService.ExistOrganizationUserByEmail(email);
            Assert.Equal(res.Value, result);
            Assert.Equal(res.IsFailure, isError);
        }
    }
}