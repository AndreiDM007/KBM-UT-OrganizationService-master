using System;
using System.Collections.Generic;
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
    public class UpdateOrganizationAdminProfileRollbackSubscriber : SagaRollbackSubscriber<UpdateOrganizationAdminProfileRollbackEvent>
    {
        private IProfileRepository ProfileRepository => serviceProvider.GetService<IProfileRepository>();

        public UpdateOrganizationAdminProfileRollbackSubscriber(
            ILogger logger,
            IMessageSubscriber messageSubscriber,
            IServiceProvider serviceProvider,
            IMessagePublisher publisher) : base(logger, messageSubscriber, serviceProvider, publisher)
        {
        }

        protected override bool HandleMessage(UpdateOrganizationAdminProfileRollbackEvent content)
        {
            IEnumerable<ProfileValueUpdateModel> fullList = new[]
            {
                new ProfileValueUpdateModel {ProfileParameterId = 1, Value = content.FirstName},
                new ProfileValueUpdateModel {ProfileParameterId = 2, Value = content.LastName},
                new ProfileValueUpdateModel {ProfileParameterId = 16, Value = content.Email},
                new ProfileValueUpdateModel {ProfileParameterId = 17, Value = content.Phone}
            };
            var filteredList = fullList.Where(pv => !string.IsNullOrEmpty(pv.Value)).ToList();

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