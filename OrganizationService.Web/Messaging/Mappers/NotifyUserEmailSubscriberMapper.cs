using AutoMapper;
using Kebormed.Core.OrganizationService.Messaging.Messages;
using Kebormed.Core.OrganizationService.Web.Business.Services.Email.Models;

namespace Kebormed.Core.OrganizationService.Web.Messaging.Mappers
{
    public class NotifyUserEmailSubscriberMapper
    {
        public IMapper Mapper;

        public NotifyUserEmailSubscriberMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<NotifyUserEmailDomainEvent, SendEmailCommand>();
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}