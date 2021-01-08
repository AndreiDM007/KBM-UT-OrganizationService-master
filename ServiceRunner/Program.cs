using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile;
using Kebormed.Core.OrganizationService.Web.Data;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrganizationService.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var builtDependencies = MockDependencyInjectionObjects();

            var profileServiceTest = builtDependencies.GetService<ProfileServiceTests>();
            var organizationUserServiceTests = builtDependencies.GetService<OrganizationUserServiceTests>();

            profileServiceTest.TestAll();
            organizationUserServiceTests.TestAll();

        }

        private static ServiceProvider MockDependencyInjectionObjects()
        {
            var container = new ServiceCollection();
            container.AddSingleton<OrganizationServiceDataContextFactory>();

            container.AddDbContext<OrganizationServiceDataContext>(options =>
                options.UseSqlServer(
                    "Server=tcp:127.0.0.1,1433;Initial Catalog=core_OrganizationService;Persist Security Info=False;User ID=sa;Password=123.mssql.321;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=Yes;Connection Timeout=30;"));
            // Register repositories
            container.AddScoped<ProfileRepository>();
            container.AddScoped<ProfileDataMapper>();

            container.AddScoped<OrganizationUserRepository>();
            container.AddScoped<OrganizationUserDataMapper>();           

            // Register services
            container.AddScoped<ProfileService>();
            container.AddScoped<ProfileGrpcMapper>();

            container.AddScoped<OrganizationUserService>();
            container.AddScoped<OrganizationUserGrpcMapper>();


            //Test objects registration
            container.AddScoped<ProfileServiceTests>();
            container.AddScoped<OrganizationUserServiceTests>();

            return container.BuildServiceProvider();
        }
    }
}
