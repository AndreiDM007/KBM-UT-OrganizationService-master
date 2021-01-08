using System;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages.Sagas;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.Saga.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas
{
    public class
        CreateOrganizationUserProfileRollbackSubscriber : SagaRollbackSubscriber<
            CreateOrganizationUserProfileRollbackEvent>
    {
        private IProfileRepository ProfileRepository => serviceProvider.GetService<IProfileRepository>();

        public CreateOrganizationUserProfileRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(CreateOrganizationUserProfileRollbackEvent content)
        {
            var organizationUserProfileId = content.OrganizationUserProfileId;
            var transactionId = content.TransactionId;

            ProfileRepository.RollbackCreateOrganizationUserProfile(transactionId);
            logger.LogInformation("rollback the creation of the organization admin profile with {0}",
                organizationUserProfileId);

            return true;
        }
    }
}