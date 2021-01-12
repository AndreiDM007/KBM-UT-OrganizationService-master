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
        [InlineData(1, 2, true)]
        [InlineData(2, 3, false)]
        public void OrganizationUserExists_ByOrganizationIdAndUserId_Test(int organizationId, int organizationUserId, bool result)
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
        public void OrganizationUserExists_ByOrganizationIdAndOrganizationUser_Test(int organizationId, int organizationUserId, bool result)
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
        public void OrganizationUserExistsByEmail_Test(string email, bool result)
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
        [InlineData(1, 2, 1)]
        //[InlineData(2, 2, 2)]
        public void GetOrganizationUserType_Test(int organizationUserId, int organizationId, int? result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationUserEntityId = 1,
                    OrganizationId = 2,
                    Organization = new OrganizationEntity(),
                    UserType = 1,
                }
            });

            var res = organizationUserRepository.GetOrganizationUserType(organizationUserId, organizationId);

            Assert.Equal(res, result);
        }

        [Theory]
        [InlineData("user1", true)]
        [InlineData("user2", false)]
        public void OrganizationUserExistsByUserName_Test(string userName, bool result)
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
        public void CreateOrganizationUser_Test()
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

        [Fact]
        public void UpdateOrganizationUser_ThrowErrorWhenNull_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>());

            //var command = new UpdateOrganizationUserCommand
            //{
            //    OrganizationId = 1,
            //    OrganizationUserId = 5,
            //    UserType = 2,
            //    FirstName = "user1",
            //    LastName = "User1",
            //    Email = "user1@email.com",
            //    IsActive = true,
            //    TransactionId = "abc1"
            //};



            //organizationUserRepository.UpdateOrganizationUser(command);

            Assert.Throws<NullReferenceException>(() =>
                organizationUserRepository.UpdateOrganizationUser(null));
        }

        [Fact]
        public void UpdateOrganizationUser_Email_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>(new[] {
                new OrganizationUserEntity
                    {
                        OrganizationUserEntityId = 5,
                        OrganizationId = 1,
                        UserType = 2,
                        FirstName = "user0",
                        LastName = "User0",
                        Email = "user0@email.com",
                        IsActive = true,
                        TransactionId = "abc0"
                    }
                }));

            var command = new UpdateOrganizationUserCommand
            {
                OrganizationId = 1,
                OrganizationUserId = 5,
                UserType = 2,
                FirstName = "user1",
                LastName = "User1",
                Email = "user1@email.com",
                IsActive = true,
                TransactionId = "abc1"
            };



            organizationUserRepository.UpdateOrganizationUser(command);
            var result = organizationUserRepository.GetOrganizationUser(command.OrganizationUserId, command.UserType, command.OrganizationId);


            Assert.Equal(command.Email, result.Email);

        }

        [Fact]
        public void UpdateOrganizationUser_Name_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>(new[] {
                new OrganizationUserEntity
                    {
                        OrganizationUserEntityId = 5,
                        OrganizationId = 1,
                        UserType = 2,
                        FirstName = "user0",
                        LastName = "User0",
                        Email = "user0@email.com",
                        IsActive = true,
                        TransactionId = "abc0"
                    }
                }));

            var command = new UpdateOrganizationUserCommand
            {
                OrganizationId = 1,
                OrganizationUserId = 5,
                UserType = 2,
                FirstName = "user1",
                LastName = "User1",
                Email = "user1@email.com",
                IsActive = true,
                TransactionId = "abc1"
            };



            organizationUserRepository.UpdateOrganizationUser(command);
            var result = organizationUserRepository.GetOrganizationUser(command.OrganizationUserId, command.UserType, command.OrganizationId);


            Assert.Equal(command.FirstName, result.FirstName);
            Assert.Equal(command.LastName, result.LastName);
            Assert.Equal(command.IsActive, result.IsActive);

        }

        [Theory]
        [InlineData(5, 2, 1)]
        //[InlineData(2, 2, 2)]
        public void DeleteOrganizationUser_Success_Test(int organizationUserId, int userType, int organizationId)
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);

            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    OrganizationUserEntityId = 5,
                    UserType = 2,
                    FirstName = "user1",
                    LastName = "User1",
                    Email = "user1@email.com",
                    IsActive = true,
                    TransactionId = "abc1"

                }
            });

            var associatedUsers = Ext.MockDbSet(new List<AssociatedOrganizationUserEntity> { new AssociatedOrganizationUserEntity() });
            context.Setup(ctx => ctx.AssociatedOrganizationUserEntities).Returns(associatedUsers.Object);

            var res = organizationUserRepository.DeleteOrganizationUser(organizationUserId, userType, organizationId);

            Assert.True(res > 0);
        }

        [Theory]
        [InlineData("user1@email.com", 2, false)]
        public void IsEmailInUse_Test(string email, int? organizationUserId, bool result)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    Email = "user1@email.com",
                    OrganizationUserEntityId = 2,
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.IsEmailInUse(email, organizationUserId);

            Assert.Equal(res, result);
        }

        [Fact]
        public void GetOrganizationUser_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity> {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    OrganizationUserEntityId = 5,
                    UserType = 2,
                    FirstName = "user1",
                    LastName = "User1",
                    Email = "user1@email.com",
                    IsActive = true,
                    TransactionId = "abc1"
                }
            });

            var command = new OrganizationUserEntity
            {
                OrganizationId = 1,
                OrganizationUserEntityId = 5,
                UserType = 2,
            };


            var res = organizationUserRepository.GetOrganizationUser(command.OrganizationUserEntityId, command.UserType, command.OrganizationId);



            Assert.NotNull(res);
        }

        [Fact]
        public void GetOrganizationUserByExternalUserId_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity> {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    OrganizationUserEntityId = 5,
                    UserType = 2,
                    FirstName = "user1",
                    LastName = "User1",
                    Email = "user1@email.com",
                    IsActive = true,
                    TransactionId = "abc1",
                    UserId = "abc",
                    Username = "Andrei"
                }
            });

            var command = new OrganizationUserEntity
            {
                OrganizationId = 1,
                OrganizationUserEntityId = 5,
                UserType = 2,
                UserId = "abc"
            };


            var res = organizationUserRepository.GetOrganizationUserByExternalUserId(command.OrganizationId, command.UserId);



            Assert.NotNull(res);
        }

        [Fact]
        public void GetOrganizationAdmin_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity> {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    OrganizationUserEntityId = 5,
                    UserType = 2,
                    FirstName = "user1",
                    LastName = "User1",
                    Email = "user1@email.com",
                    IsActive = true,
                    TransactionId = "abc1",
                    UserId = "abc",
                    Username = "Andrei"
                }
            });

            var command = new OrganizationUserEntity
            {
                OrganizationId = 1,
                OrganizationUserEntityId = 5,
                UserType = 2,
                UserId = "abc"
            };


            var res = organizationUserRepository.GetOrganizationAdmin(command.OrganizationId, command.UserType);



            Assert.NotNull(res);
        }

        [Theory]
        [InlineData("abc", true)]
        //[InlineData("abd", true)]
        public void SetOrganizationUserLockedStatus_Test(string externaluserId, bool isLocked)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    IsLocked = false,
                    OrganizationUserEntityId = 2,
                    UserId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.SetOrganizationUserLockedStatus(externaluserId, isLocked);

            Assert.Equal(res.IsLocked, isLocked);
        }

        [Theory]
        [InlineData("abc", true)]
        //[InlineData("abd", true)]
        public void SetUserPendingActivationStatus_Test(string externaluserId, bool isPendingActivation)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {
                    OrganizationId = 1,
                    IsPendingActivation = false,
                    OrganizationUserEntityId = 2,
                    UserId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.SetUserPendingActivationStatus(externaluserId, isPendingActivation);

            Assert.Equal(res.IsPendingActivation, isPendingActivation);
        }

        [Theory]
        [InlineData("abc", 100000000)]
        //[InlineData("abd", true)]
        public void SetLastLoginTime_Test(string externaluserId, long loginTimestamp)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {

                    OrganizationId = 1,
                    LastLoginAt = 100000010,
                    OrganizationUserEntityId = 2,
                    UserId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.SetLastLoginTime(externaluserId, loginTimestamp);

            Assert.Equal(res.LastLoginAt, loginTimestamp);
        }

        [Theory]
        [InlineData(22)]
        //[InlineData("abd", true)]
        public void AnyOrganizationUserWithSharedUserId_Test(int organizationUserId)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {

                    OrganizationId = 22,
                    LastLoginAt = 100000010,
                    OrganizationUserEntityId = 22,
                    UserId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.AnyOrganizationUserWithSharedUserId(organizationUserId);

            Assert.NotNull(res);
        }

        [Theory]
        [InlineData(23)]
        //[InlineData("abd", true)]
        public void GetSingleOrganizationOrgUsers_Test(int organizationId)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {

                    OrganizationId = 23,
                    LastLoginAt = 100000010,
                    //OrganizationUserEntityId = 22,
                    UserId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.GetSingleOrganizationOrgUsers(organizationId);

            Assert.True(res.Result.Count > 0);
        }

        [Theory]
        [InlineData("abc")]
        //[InlineData("abd", true)]
        public void RollbackCreateOrganizationUser_Test(string transactionId)
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {

                    OrganizationId = 23,
                    LastLoginAt = 100000010,
                    //OrganizationUserEntityId = 22,
                    TransactionId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            organizationUserRepository.RollbackCreateOrganizationUser(transactionId);

        }


        [Theory]
        [InlineData("abc")]
        //[InlineData("abd", true)]
        public void QueryUserOrganizations_Test(string userId)
        {
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>
            {
                new OrganizationUserEntity
                {

                    OrganizationId = 23,
                    LastLoginAt = 100000010,
                    OrganizationUserEntityId = 22,
                    UserId = "abc",
                    Organization = new OrganizationEntity()
                }
            });

            var res = organizationUserRepository.QueryUserOrganizations(userId);

            Assert.NotNull(res);
        }

        [Fact]
        public void QueryOrganizationUser_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>(new[] {
                new OrganizationUserEntity
                    {
                        OrganizationUserEntityId = 5,
                        OrganizationId = 1,
                        UserType = 2,
                        FirstName = "user0",
                        LastName = "User0",
                        Email = "user0@email.com",
                        IsActive = true,
                        TransactionId = "abc0"
                    }
                }));

            var model = new QueryOrganizationUserCriteria
            {
                UserType = 2,
                //OrganizationIds = {1}
            };



            var res = organizationUserRepository.QueryOrganizationUser(model);


            Assert.True(res.Total > 0);

        }

        [Fact]
        public void QueryOrganizationUsers_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>(new[] {
                new OrganizationUserEntity
                    {
                        OrganizationUserEntityId = 5,
                        OrganizationId = 1,
                        UserType = 2,
                        FirstName = "Andrei",
                        LastName = "Daniel",
                        Email = "user0@email.com",
                        IsActive = true,
                        TransactionId = "abc0"
                    }
                }));

            var orgUserRoles = Ext.MockDbSet(new List<OrganizationUserRoleEntity> { new OrganizationUserRoleEntity() });
            context.Setup(ctx => ctx.OrganizationUserRoles).Returns(orgUserRoles.Object);

            var rolePermissions = Ext.MockDbSet(new List<RolePermissionEntity> { });
            context.Setup(ctx => ctx.RolePermissions).Returns(rolePermissions.Object);

            var criteria = new QueryOrganizationUsersCriteria
            {
                OrganizationId = 1,
                Q = "Andrei",
                OrderBy = "firstName",
                Direction = "asc",
                Page = 1,
                PageSize = 1,
                UserTypes = new List<int> { 1, 2 }
            };




            var res = organizationUserRepository.QueryOrganizationUsers(criteria);


            Assert.True(res.Result.Count > 0);

        }

        [Theory]
        [InlineData(1, 2, true)]
        //[InlineData(2, 3, false)]
        public void AssociationTypeExists_Test(int organizationUserId, int associtationType, bool result)
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);

            var associatedUsers = Ext.MockDbSet(new List<AssociatedOrganizationUserEntity> { new AssociatedOrganizationUserEntity
                {
                    OrganizationUserId1 = 1,
                    OrganizationUserId2 = 1,
                    AssociationType = 2,
                } });
            context.Setup(ctx => ctx.AssociatedOrganizationUserEntities).Returns(associatedUsers.Object);


            var res = organizationUserRepository.AssociationTypeExists(organizationUserId, associtationType);

            Assert.Equal(res, result);
        }

        [Fact]
        public void AssociationExists_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);

            var associatedUsers = Ext.MockDbSet(new List<AssociatedOrganizationUserEntity> { new AssociatedOrganizationUserEntity
                {
                    OrganizationUserId1 = 1,
                    OrganizationUserId2 = 1,
                    AssociationType = 2,
                } });
            context.Setup(ctx => ctx.AssociatedOrganizationUserEntities).Returns(associatedUsers.Object);

            var command = new CreateOrganizationUsersAssociationCommand
            {
                OrganizationUserId1 = 1,
                OrganizationUserId2 = 1,
                AssociationType = 2,
            };


            var res = organizationUserRepository.AssociationExists(command);

            Assert.True(res);
        }

        [Fact]
        public void CreateAssociationOrganizationUser_Test()
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);
            SetupOrganizationUsersDbSet(new List<OrganizationUserEntity>());

            var command = new CreateOrganizationUsersAssociationCommand
            {
                OrganizationId = 1,
                OrganizationUserId1 = 1,
                OrganizationUserId2 = 1,
                AssociationType = 2,
                TransactionId = "abc"
            };

            var associatedUsers = Ext.MockDbSet(new List<AssociatedOrganizationUserEntity> { new AssociatedOrganizationUserEntity() });
            context.Setup(ctx => ctx.AssociatedOrganizationUserEntities).Returns(associatedUsers.Object);

            var res = organizationUserRepository.CreateAssociationOrganizationUser(command);



            Assert.True(res >= 0);
        }

        [Theory]
        [InlineData(5, 2)]
        //[InlineData(2, 2, 2)]
        public void DeleteAssociationOrganizationUser_Test(int organizationUserId, int associationType)
        {
            var transactionMock = new Mock<IDbContextTransaction>();
            database.Setup(d => d.BeginTransaction()).Returns(transactionMock.Object);
            context.Setup(e => e.Database).Returns(database.Object);

            var associatedUsers = Ext.MockDbSet(new List<AssociatedOrganizationUserEntity> { new AssociatedOrganizationUserEntity
                {
                    OrganizationUserId1 = organizationUserId,
                    OrganizationUserId2 = organizationUserId,
                    AssociationType = associationType
                } }); ;
            context.Setup(ctx => ctx.AssociatedOrganizationUserEntities).Returns(associatedUsers.Object);
            var res = organizationUserRepository.DeleteAssociationOrganizationUser(organizationUserId, associationType);

            Assert.True(res >= 0);
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