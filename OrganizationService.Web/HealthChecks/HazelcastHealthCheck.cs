using System;
using System.Threading;
using System.Threading.Tasks;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;
using Kebormed.Core.Messaging;
using Kebormed.Core.Messaging.Hazelcast;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Kebormed.Core.OrganizationService.Web.HealthChecks
{
    public class HazelcastHealthCheck : IHealthCheck
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly IMessagingClient messagingClient;
        private readonly HazelcastConfig configuration;
        private readonly IHazelcastInstance hzInstance;
        public string Name => nameof(HazelcastHealthCheck);

        public HazelcastHealthCheck(IMessagePublisher messagePublisher, IMessagingClient messagingClient, HazelcastConfig configuration)
        {
            this.messagePublisher = messagePublisher;
            this.messagingClient = messagingClient;
            this.configuration = configuration;

            this.hzInstance = GetHazelcastInstance();
        }


        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var map = hzInstance.GetMap<string, string>("my-distributed-map");
                //Standard Put and Get.
                map.Put("key", "value", 100, TimeUnit.Milliseconds);
                map.Get("key");
                //Concurrent Map methods, optimistic updating
                map.PutIfAbsent("somekey", "somevalue", 100, TimeUnit.Milliseconds);
                map.Replace("key", "value", "newvalue");
                
                return Task.FromResult(HealthCheckResult.Healthy("The startup task is finished."));
            }
            catch (Exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Hazelcast seems unhealthy"));
            }                        
        }

        private IHazelcastInstance GetHazelcastInstance()
        {
            var clientConfig = new ClientConfig();
            clientConfig.GetNetworkConfig().AddAddress(configuration.Address);
            return HazelcastClient.NewHazelcastClient(clientConfig);
        }
    }
}