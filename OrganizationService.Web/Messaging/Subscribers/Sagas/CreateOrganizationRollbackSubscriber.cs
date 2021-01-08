using System;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages;
using Kebormed.Core.OrganizationService.Messaging.Messages.Sagas;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.Saga.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas
{
    public class CreateOrganizationRollbackSubscriber : SagaRollbackSubscriber<CreateOrganizationRollbackEvent>
    {
        private IOrganizationRepository OrganizationRepository => serviceProvider.GetService<IOrganizationRepository>();

        public CreateOrganizationRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(CreateOrganizationRollbackEvent content)
        {
            var organizationId = content.OrganizationId;
            var transactionId = content.TransactionId;

            OrganizationRepository.RollbackCreateOrganization(transactionId); // todo we should rename this method

            logger.LogInformation("rollback the creation of the organization with {0}", organizationId);

            return true;
        }
    }
}