using System;
using System.Collections.Generic;
using System.Linq;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Data;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using OrganizationService.Tests.Utils;
using Xunit;

namespace OrganizationService.Tests.Data.Repositories
{
    public class OrganizationUserRepositoryTest
    {
        private readonly Mock<OrganizationServiceDataContext> context;
        private readonly Mock<DatabaseFacade> database;
        private readonly Mock<OrganizationUserDataMapper> organizationUserDataMapper;
        private readonly IOrganizationUserRepository organizationUserRepository;

        private readonly DbContextOptions<OrganizationServiceDataContext> options =
            new DbContextOptionsBuilder<OrganizationServiceDataContext>()
                .UseInMemoryDatabase(databaseName: "OrganizationServiceData")
                .Options;

        public OrganizationUserRepositoryTest()
        {
            context = new Mock<OrganizationServiceDataContext>(options);
            database = new Mock<DatabaseFacade>(context.Object);
            organizationUserDataMapper = new Mock<OrganizationUserDataMapper>();
            organizationUserRepository =
                new OrganizationUserRepository(context.Object, organizationUserDataMapper.Object);
        }

        #region Tests

        [Theory]
        [InlineData(1, 2,  true)]
        [InlineData(2, 3 , false)]
        public void OrganizationUserExists_ByOrganizationIdAndUserId_Test(int organizationId, int organizationUserId,  bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    OrganizationUserEntityId = 2,
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.OrganizationUserExists(organizationUserId, organizationId);

            Assert.Equal(res, result);
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(2, 3, false)]
        public void OrganizationUserExists_ByOrganizationIdAndOrganizationUserTest(int organizationId, int organizationUserId, bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    OrganizationUserEntityId = 2,
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.OrganizationUserExists(organizationUserId, organizationId);

            Assert.Equal(res, result);
        }

        [Theory]
        [InlineData(1, 3, 2, true)]
        [InlineData(2, 3, 3, false)]
        public void OrganizationUserExists_ByOrganizationIdAndUserTypeAndOrganizationUserId_Test(int organizationId, int userType, int organizationUserId, bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    UserType = 3,
                    OrganizationUserEntityId = 2,
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.OrganizationUserExists(organizationUserId, userType, organizationId);

            Assert.Equal(res, result);
        }

        [Theory]
        [InlineData("user1Id", 3, 2, true)]
        [InlineData("user2Id", 3, 3, false)]
        public void OrganizationUserExists_ByUserIdAndUserTypeAndOrganizationId_Test(string userId, int userType, int organizationId, bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    UserId = "user1Id",
                    UserType = 3,
                    OrganizationId = 2,
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.OrganizationUserExists(userId, userType, organizationId);

            Assert.Equal(res, result);
        }


        [Theory]
        [InlineData("user1@email.com", true)]
        [InlineData("user2@email.com", false)]
        public void OrganizationUserExistsByEmailTest(string email, bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    Email = "user1@email.com",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.OrganizationUserExistsByEmail(email);

            Assert.Equal(res, result);
        }

        [Theory]
        [InlineData("user1", true)]
        [InlineData("user2", false)]
        public void OrganizationUserExistsByUserName(string userName, bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    Username = "user1",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.OrganizationUserExistsByUsername(userName);

            Assert.Equal(res, result);
        }

        [Fact]
        public void CreateOrganizationUserTest()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>());

            var command = new CreateOrganizationUserCommand
            {
                UserId = "qwerty",
                OrganizationId = 1,
                UserType = 2,
                Username = "user1",
                FirstName = "user1",
                LastName = "User1",
                Email = "user1@email.com",
                IsActive = true,
                IsLocked = false,
                IsPendingActivation = false,
                TransactionId = "abc"
            };

            organizationUserRepository.CreateOrganizationUser(command);

            Assert.Throws<NullReferenceException>(() =>
                organizationUserRepository.CreateOrganizationUser(null));
        }

        #endregion

        #region Private

        private void SetupOrganizationUsersDbSet(
            List<OrganizationUserEntity> organizationUsers)
        {
            var orgUsersMock = new Mock<DbSet<OrganizationUserEntity>>();
            orgUsersMock.SetupIQueryable(organizationUsers.AsQueryable());

            context.Setup(ctx => ctx.OrganizationUsers)
                .Returns(orgUsersMock.Object);
        }

        #endregion
    }
}