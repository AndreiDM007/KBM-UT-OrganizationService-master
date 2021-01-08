using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class UserInvitationDataMapper
    {
        public IMapper Mapper;

        public UserInvitationDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<CreateUserInvitationCommand, UserInvitationEntity>()
                    .ForMember(x => x.UserInvitationEntityId, x => x.Ignore())
                    .ForMember(x => x.OrganizationUser, x => x.Ignore())
                    .ForMember(x => x.InvitationGuid, x => x.Ignore())
                    .ForMember(x => x.CreatedAt, x => x.Ignore())
                    .ForMember(x => x.AcceptedAt, x => x.Ignore())
                    .ForMember(x => x.DeclinedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())                    
                    ;

                config
                    .CreateMap<UserInvitationEntity, GetUserInvitationResult>()
                    .ForMember(x => x.OrganizationName, x => x.Ignore())
                    ;
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}