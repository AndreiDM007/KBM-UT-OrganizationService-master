using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Jaeger;
using Jaeger.Samplers;
using Kebormed.Core.Communication.Grpc.Configuration;
using Kebormed.Core.Messaging;
using Kebormed.Core.Messaging.Hazelcast;
using Kebormed.Core.OrganizationService.Web.Business.Services.Email;
using Kebormed.Core.OrganizationService.Web.Configuration;
using Kebormed.Core.OrganizationService.Web.Data;
using Kebormed.Core.OrganizationService.Web.Grpc.Controllers;
using Kebormed.Core.OrganizationService.Web.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Contrib.Grpc.Interceptors;
using OpenTracing.Util;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SimpleInjector;
using HealthCheckResult = Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult;
using IHealthCheck = Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck;


namespace Kebormed.Core.OrganizationService.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder()
                .Build()
                .MigrateDatabase<OrganizationServiceDataContext>()
                .ConfigureHealthCheck(new List<Type>
                {
                    typeof(HazelcastHealthCheck)
                })
                .Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {            
            var hzConfiguration = new HazelcastConfig();
            var emailSettings = new EmailSettings();
            return new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {                    
                    var configurationRoot = configurationBuilder
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();
                    configurationRoot.Bind(nameof(HazelcastConfig), hzConfiguration);
                    configurationRoot.Bind(nameof(EmailSettings), emailSettings);
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.UseLog4Net(new FileInfo("log4net.config"));
                })
                .ConfigureSimpleInjector()
                .ConfigureContainer<Container>((hostBuilderContext, container) =>
                {
                    container.RegisterSingleton(() => hzConfiguration);
                    
                    // Register IMessagingClientFactory
                    container.RegisterSingleton<IMessagingClientFactory, HazelcastMessagingClientFactory>();

                    // Register IMessagingClient
                    container.RegisterSingleton(() =>
                    {
                        var config = container.GetInstance<IConfiguration>();
                        var factory = container.GetInstance<IMessagingClientFactory>();
                        return factory.CreateClient(nameof(HazelcastConfig), config);
                    });

                    // Register IMessagePublisher
                    container.RegisterSingleton<IMessagePublisher>(container.GetInstance<IMessagingClient>);

                    // Register IMessageSubscriber
                    container.RegisterSingleton<IMessageSubscriber>(container.GetInstance<IMessagingClient>);

                    // Register data context
                    container.RegisterSingleton(() => new OrganizationServiceDataContextFactory { LoggerFactory = container.GetInstance<ILoggerFactory>() });
                    
                    //Register Email Service
                    container.RegisterSingleton(() => new EmailService(emailSettings));

                    container.RegisterScoped(() =>
                    {
                        var factory = container.GetInstance<OrganizationServiceDataContextFactory>();
                        var connectionString = hostBuilderContext.Configuration.GetConnectionString(nameof(OrganizationServiceDataContext));
                        return factory.CreateDbContext(connectionString);
                    });

                   // Register repositories
                    DependencyInjectionRegistration.RegisterRepositories(container);

                    // Register services
                    DependencyInjectionRegistration.RegisterServices(container);

                    //Register subscribers
                    DependencyInjectionRegistration.RegisterSubscribers(container);

                    //Register healthchecks 
                    DependencyInjectionRegistration.RegisterHealthChecks(container);
                })
                .AddGrpcService<ProfileController>()
                .AddGrpcService<OrganizationController>()
                .AddGrpcService<OrganizationUserController>()
                .AddGrpcService<GroupController>()
                .AddGrpcService<GroupAuthorizationController>()
                .AddGrpcService<AuthorizationController>()
                .AddGrpcService<UserInvitationController>()
                .AddGrpcService<HealthController>()
                .ConfigureGrpcServer(
                    Assembly.GetEntryAssembly().GetName().Name,
                    (hostBuilderContext) =>
                    {
                        var config = new TracingConfig();
                        hostBuilderContext.Configuration.Bind(nameof(TracingConfig), config);
                        return config;
                    },
                    (hostBuilderContext, c, logger, serverServiceDefinitions, interceptors) =>
                    {
                        var server = new Server();
                        var config = new GrpcConfig();

                        hostBuilderContext.Configuration.Bind(nameof(GrpcConfig), config);

                        server.Ports.Add(new ServerPort(config.Host, config.Port, ServerCredentials.Insecure));

                        GrpcEnvironment.SetLogger(logger);

                        serverServiceDefinitions
                            .ToList()
                            .ForEach(x => { server.Services.Add(x.Intercept(interceptors.ToArray())); });

                        return server;
                    })
                .ConfigureContainer<Container>(container => container.Verify());
        }
    }
}