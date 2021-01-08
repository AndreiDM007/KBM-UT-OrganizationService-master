using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group;
using Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile;
using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;
using Kebormed.Core.OrganizationService.Web.HealthChecks;
using Kebormed.Core.OrganizationService.Web.Messaging.Mappers;
using Kebormed.Core.OrganizationService.Web.Messaging.Subscribers;
using Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.NotifyUserEmail;
using Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas;
using Microsoft.Extensions.Hosting;
using SimpleInjector;

namespace Kebormed.Core.OrganizationService.Web.Configuration
{
    public static class DependencyInjectionRegistration
    {
        public static void RegisterServices(Container container)
        {
            container.RegisterScoped<ProfileService>();
            container.RegisterScoped<ProfileGrpcMapper>();

            container.RegisterScoped<OrganizationUserService>();
            container.RegisterScoped<OrganizationUserGrpcMapper>();

            container.RegisterScoped<Business.Services.Organization.OrganizationService>();
            container.RegisterScoped<OrganizationGrpcMapper>();

            container.RegisterScoped<GroupService>();
            container.RegisterScoped<GroupGrpcMapper>();


            container.RegisterScoped<GroupAuthorizationService>();
            container.RegisterScoped<GroupAuthorizationGrpcMapper>();

            container.RegisterScoped<AuthorizationService>();
            container.RegisterScoped<AuthorizationGrpcMapper>();
            
            container.RegisterScoped<UserInvitationService>();
            container.RegisterScoped<UserInvitationGrpcMapper>();
        }

        public static void RegisterRepositories(Container container)
        {
            //When initializing a new class please use interfaces instead of the implementations.
            //Gradually ScopedRegistrations just for the implementation should be removed

            container.RegisterScoped<IProfileRepository, ProfileRepository>();
            container.RegisterScoped<ProfileDataMapper>();

            container.RegisterScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            container.RegisterScoped<OrganizationUserDataMapper>();

            container.RegisterScoped<IOrganizationRepository, OrganizationRepository>();
            container.RegisterScoped<OrganizationDataMapper>();

            container.RegisterScoped<IGroupRepository, GroupRepository>();
            container.RegisterScoped<GroupDataMapper>();

            container.RegisterScoped<IGroupAuthorizationRepository, GroupAuthorizationRepository>();

            container.RegisterScoped<IRolePermissionRepository, RolePermissionRepository>();
            container.RegisterScoped<RolePermissionDataMapper>();

            container.RegisterScoped<IOrganizationUserPermissionRepository, OrganizationUserPermissionRepository>();
            container.RegisterScoped<OrganizationUserPermissionDataMapper>();

            container.RegisterScoped<IOrganizationUserRoleRepository, OrganizationUserRoleRepository>();
            container.RegisterScoped<OrganizationUserRoleDataMapper>();
            
            container.RegisterScoped<IUserInvitationRepository, UserInvitationRepository>();
            container.RegisterScoped<UserInvitationDataMapper>();
        }

        public static void RegisterSubscribers(Container container)
        {           
            // --> create tenant saga
            container.Collection.Append<IHostedService, CreateOrganizationRollbackSubscriber>(Lifestyle.Singleton);
            container.Collection.Append<IHostedService, CreateOrganizationUserRollbackSubscriber>(Lifestyle.Singleton);
            container.Collection.Append<IHostedService, CreateOrganizationAdminProfileRollbackSubscriber>(Lifestyle.Singleton);
            container.Collection.Append<IHostedService, CreateSuperAdminRollbackSubscriber>(Lifestyle.Singleton);
            // <-- create tenant saga
            
            container.Collection.Append<IHostedService, CreateOrganizationUserProfileRollbackSubscriber>(Lifestyle.Singleton);

            // --> update tenant admin saga
            container.Collection.Append<IHostedService, UpdateOrganizationUserRollbackSubscriber>(Lifestyle.Singleton);
            container.Collection.Append<IHostedService, UpdateOrganizationAdminProfileRollbackSubscriber>(Lifestyle.Singleton);
            // <-- update tenant admin saga

            // --> update patient saga
            container.Collection.Append<IHostedService, UpdatePatientProfileRollbackSubscriber>(Lifestyle.Singleton);
            // <-- update patient saga
            
            // --> update physician saga
            container.Collection.Append<IHostedService, UpdatePhysicianProfileRollbackSubscriber>(Lifestyle.Singleton);
            // <-- update physician saga
            
            container.Collection.Append<IHostedService, NotifyUserEmailSubscriber>(Lifestyle.Singleton);
            container.RegisterSingleton<NotifyUserEmailSubscriberMapper>();
        }

        public static void RegisterHealthChecks(Container container)
        {
            container.RegisterSingleton<HazelcastHealthCheck>();
        }
    }
}