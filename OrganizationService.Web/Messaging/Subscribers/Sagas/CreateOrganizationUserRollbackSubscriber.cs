using System;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages;
using Kebormed.Core.OrganizationService.Messaging.Messages.Sagas;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.Saga.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas
{
    public class CreateOrganizationUserRollbackSubscriber : SagaRollbackSubscriber<CreateOrganizationUserRollbackEvent>
    {
        private IOrganizationUserRepository OrganizationUserRepository => serviceProvider.GetService<IOrganizationUserRepository>();

        public CreateOrganizationUserRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(CreateOrganizationUserRollbackEvent content)
        {
            var organizationUserId = content.OrganizationUserId;
            var userType = content.UserType;
            var organizationId = content.OrganizationId;
            var transactionId = content.TransactionId;

            OrganizationUserRepository.RollbackCreateOrganizationUser(transactionId);

            logger.LogInformation("rollback the creation of the organization user with {0}", organizationUserId);

            return true;
        }
    }
}