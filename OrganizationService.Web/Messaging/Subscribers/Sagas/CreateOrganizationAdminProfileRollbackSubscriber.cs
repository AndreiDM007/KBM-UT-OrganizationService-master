using System;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages.Sagas;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.Saga.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas
{
    public class CreateOrganizationAdminProfileRollbackSubscriber : SagaRollbackSubscriber<CreateOrganizationAdminProfileRollbackEvent>
    {
        private IProfileRepository ProfileRepository => serviceProvider.GetService<IProfileRepository>();

        public CreateOrganizationAdminProfileRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(CreateOrganizationAdminProfileRollbackEvent content)
        {
            var organizationAdminProfileId = content.OrganizationAdminProfileId;
            var transactionId = content.TransactionId;

            ProfileRepository.RollbackCreateOrganizationUserProfile(transactionId); 
            logger.LogInformation("rollback the creation of the organization admin profile with {0}", organizationAdminProfileId);

            return true;
        }
    }
}