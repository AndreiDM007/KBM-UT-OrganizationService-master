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
    public class CreateSuperAdminRollbackSubscriber : SagaRollbackSubscriber<CreateSuperAdminRollbackEvent>
    {
        private IOrganizationUserRepository OrganizationUserRepository => serviceProvider.GetService<IOrganizationUserRepository>();

        public CreateSuperAdminRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(CreateSuperAdminRollbackEvent content)
        {
            var superAdminOrganizationUserId = content.SuperAdminOrganizationUserId;
            var transactionId = content.TransactionId;

            // todo we should use the superAdminOrganizationUserId !!!
            OrganizationUserRepository.RollbackCreateOrganizationUser(transactionId);

            logger.LogInformation("rollback the creation of the super admin org user with {}", superAdminOrganizationUserId);
            return true;
        }
    }
}