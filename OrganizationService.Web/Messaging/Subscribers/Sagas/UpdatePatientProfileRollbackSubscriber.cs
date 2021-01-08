using System;
using System.Linq;
using Kebormed.Core.Messaging;
using Kebormed.Core.OrganizationService.Messaging.Messages.Sagas;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;
using Kebormed.Core.Saga.Messaging.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Subscribers.Sagas
{
    public class UpdatePatientProfileRollbackSubscriber : SagaRollbackSubscriber<UpdatePatientProfileRollbackEvent>
    {
        private IProfileRepository ProfileRepository => serviceProvider.GetService<IProfileRepository>();

        public UpdatePatientProfileRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(UpdatePatientProfileRollbackEvent content)
        {
            var filteredList = content.ProfileValues
                .Select(x => new ProfileValueUpdateModel {ProfileParameterId = x.Key, Value = x.Value})
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .ToList();

            ProfileRepository.UpdateProfile(new UpdateProfileCommand
                {
                    OrganizationUserId = content.OrganizationUserId,
                    OrganizationId = content.OrganizationId,
                    ProfileValues = filteredList
                }
            );
            return true;
        }
    }
}