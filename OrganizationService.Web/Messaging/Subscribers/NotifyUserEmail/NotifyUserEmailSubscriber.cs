using System;
using Kebormed.Core.Messaging;
using Kebormed.Core.Messaging.Hazelcast;
using Kebormed.Core.OrganizationService.Messaging.Messages;
using Kebormed.Core.OrganizationService.Web.Business.Services.Email;
using Kebormed.Core.OrganizationService.Web.Business.Services.Email.Models;
using Kebormed.Core.OrganizationService.Web.Messaging.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.NotifyUserEmail
{
    public class NotifyUserEmailSubscriber : TypedQueueSubscriber<NotifyUserEmailDomainEvent>
    {
        private EmailService EmailService => serviceProvider.GetService<EmailService>();

        public NotifyUserEmailSubscriber(
            ILogger logger,
            NotifyUserEmailSubscriberMapper notifyUserEmailSubscriberMapper,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider) : base(logger, notifyUserEmailSubscriberMapper.Mapper, messageSubscriber, serviceProvider)
        {
            handlers.Add(NotifyUserHandler);
        }

        private void NotifyUserHandler(IMessage<NotifyUserEmailDomainEvent> message)
        {
            logger.LogTrace("User Email Notification message received");
            var content = message.Content;

            var sendEmailCommand = mapper.Map<SendEmailCommand>(content);
            EmailService.SendEmailAsync(sendEmailCommand);
            logger.LogTrace("User Email Notification handling finished.");
        }
    }
}