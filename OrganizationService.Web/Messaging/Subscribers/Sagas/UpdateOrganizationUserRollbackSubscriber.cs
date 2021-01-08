using System;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages.Sagas;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.Saga.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas
{
    public class UpdateOrganizationUserRollbackSubscriber : SagaRollbackSubscriber<UpdateOrganizationUserRollbackEvent>
    {
        private IOrganizationUserRepository OrganizationUserRepository => serviceProvider.GetService<IOrganizationUserRepository>();

        public UpdateOrganizationUserRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(UpdateOrganizationUserRollbackEvent content)
        {
            OrganizationUserRepository.UpdateOrganizationUser(new UpdateOrganizationUserCommand
            {
                OrganizationId = content.OrganizationId,
                OrganizationUserId = content.OrganizationUserId,
                Email = content.Email,
                UserType = content.UserType,
                FirstName = content.FirstName,
                LastName = content.LastName,
                IsActive = content.IsActive
            });

            return true;
        }
    }
}