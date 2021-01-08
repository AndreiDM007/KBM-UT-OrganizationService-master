using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Profile.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class ProfileDataMapper
    {
        public IMapper Mapper { get; }

        public ProfileDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<CreateProfileCommand, ProfileEntity>()
                    .ForMember(x => x.ProfileEntityId, x => x.Ignore())
                    .ForMember(x => x.OrganizationUser, x => x.Ignore())
                    .ForMember(x => x.RollbackedAt, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())
                    ;                    
                config
                    .CreateMap<ProfileValueCreateModel, ProfileValueEntity>()
                    .ForMember(d => d.TransactionId, d => d.MapFrom(s => s.TransactionId))
                    .ForMember(x => x.ProfileId, x => x.Ignore())
                    .ForMember(x => x.ProfileParameter, x => x.Ignore())
                    .ForMember(x => x.ProfileValueEntityId, x => x.Ignore())
                    .ForMember(x => x.Profile, x => x.Ignore())                    
                    .ForMember(x => x.RollbackedAt, x => x.Ignore())                    
                    ;
                config
                    .CreateMap<UpdateProfileCommand, ProfileEntity>()
                    .ForMember(d => d.TransactionId, d => d.Ignore())
                    .ForMember(x => x.ProfileEntityId, x => x.Ignore())
                    .ForMember(x => x.OrganizationUser, x => x.Ignore())
                    .ForMember(x => x.RollbackedAt, x => x.Ignore())
                    .ForMember(x => x.UpdatedAt, x => x.Ignore())
                    .ForMember(x => x.DeletedAt, x => x.Ignore())
                    ;
                config
                    .CreateMap<ProfileValueUpdateModel, ProfileValueEntity>()
                    .ForMember(d => d.TransactionId, d => d.Ignore())
                    .ForMember(x => x.ProfileId, x => x.Ignore())
                    .ForMember(x => x.ProfileParameter, x => x.Ignore())
                    .ForMember(x => x.ProfileValueEntityId, x => x.Ignore())
                    .ForMember(x => x.Profile, x => x.Ignore())
                    .ForMember(x => x.RollbackedAt, x => x.Ignore())
                    ;
                config.CreateMap<ProfileEntity, GetProfileResult>()
                    .ForMember(d => d.ProfileId, d => d.MapFrom(s => s.ProfileEntityId))
                    ;
                config.CreateMap<ProfileValueEntity, ProfileValuesListModel>();
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}
