using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;
using EFEntity = Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class OrganizationUserDataMapper
    {
        public IMapper Mapper { get; }

        public OrganizationUserDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<EFEntity.OrganizationUserEntity, GetOrganizationUserResult>()
                    .ForMember(o => o.OrganizationUserId, d => d.MapFrom(e => e.OrganizationUserEntityId))
                    .ForMember(o => o.ProfileId, d => d.MapFrom(e => e.Profile.ProfileEntityId))
                    .ForMember(o => o.ProfileValues, d => d.MapFrom(e => e.Profile.ProfileValues))
                    .ForMember(o => o.Groups, d => d.MapFrom(e => e.Groups))
                    .ForMember(o => o.HasAcceptedInvitation, d => d.Condition(e => e.UserInvitation != null))
                    .ForMember(o => o.HasAcceptedInvitation, d => d.MapFrom(e => e.UserInvitation.AcceptedAt.HasValue))
                    ;
                
                config
                    .CreateMap<EFEntity.OrganizationUserEntity, GetOrganizationUserByExternalUserIdResult>()
                    .ForMember(o => o.OrganizationUserId, d => d.MapFrom(e => e.OrganizationUserEntityId))
                    .ForMember(o => o.Username, d => d.MapFrom(e => e.Username))
                    .ForMember(o => o.UserType, d => d.MapFrom(e => e.UserType))
                    ;
                
                config
                    .CreateMap<EFEntity.OrganizationUserEntity, GetOrganizationAdminResult>()
                    .ForMember(o => o.OrganizationUserId, d => d.MapFrom(e => e.OrganizationUserEntityId))
                    ;

                config.CreateMap<EFEntity.GroupMemberEntity, OrganizationUserGroupListModel>()
                    .ForMember(d => d.GroupId, d => d.MapFrom(s => s.Group.GroupEntityId))
                    .ForMember(d => d.GroupName, d => d.MapFrom(s => s.Group.Name))
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));               
                
                config.CreateMap<EFEntity.ProfileValueEntity, ProfileValuesListModel>();
                
                config.CreateMap<EFEntity.AssociatedOrganizationUserEntity, AssociationOrganizationUserListModel>()
                    .ForMember(d => d.OrganizationUserId, d => d.MapFrom(s => s.OrganizationUserId2));

                config
                    .CreateMap<CreateOrganizationUserCommand, EFEntity.OrganizationUserEntity>()
                    .ForMember(o => o.OrganizationUserEntityId, d => d.Ignore())
                    .ForMember(o => o.Organization, d => d.Ignore())
                    .ForMember(o => o.AssociatedOrganizationUsers, d => d.Ignore())
                    .ForMember(o => o.UserInvitation, d => d.Ignore())
                    .ForMember(o => o.Profile, d => d.Ignore())
                    .ForMember(o => o.RollbackedAt, d => d.Ignore())
                    .ForMember(o => o.DeletedAt, d => d.Ignore())
                    .ForMember(o => o.CreatedAt, d => d.Ignore())
                    .ForMember(o => o.UpdatedAt, d => d.Ignore())
                    .ForMember(o => o.Groups, d => d.Ignore())
                    .ForMember(o => o.LastLoginAt, d => d.Ignore())
                    .ForMember(o => o.Roles, d => d.Ignore())
                    ;
                config
                    .CreateMap<EFEntity.OrganizationUserEntity, EFEntity.OrganizationUserEntity>()
                    .ForMember(o => o.OrganizationUserEntityId, d => d.Ignore())
                    ;
                config
                    .CreateMap<EFEntity.OrganizationUserEntity, OrganizationUserListModel>()
                    .ForMember(o => o.OrganizationUserId, d => d.MapFrom(e => e.OrganizationUserEntityId))
                    .ForMember(o => o.ProfileId, d => d.MapFrom(e => e.Profile.ProfileEntityId))
                    .ForMember(o => o.ProfileValues, d => d.MapFrom(e => e.Profile.ProfileValues))             
                    ;

                config
                    .CreateMap<EFEntity.OrganizationUserEntity, OrganizationUsersListModel>()
                    .ForMember(o => o.OrganizationUserId, d => d.MapFrom(e => e.OrganizationUserEntityId))                                        
                    .ForMember(o => o.Roles, d => d.Ignore())                                        
                    .ForMember(o => o.LastLoginTime, d => d.MapFrom(e => e.LastLoginAt))
                    ;

                //associations
                config
                    .CreateMap<CreateOrganizationUsersAssociationCommand, EFEntity.AssociatedOrganizationUserEntity>()
                    .ForMember(d => d.AssociatedOrganizationUserEntityId, d => d.Ignore())
                    .ForMember(d => d.OrganizationUser, d => d.Ignore())
                    .ForMember(d => d.RollbackedAt, d => d.Ignore())
                    .ForMember(d => d.CreatedAt, d => d.Ignore())
                    .ForMember(d => d.CreatedBy, d => d.Ignore())
                    .ForMember(d => d.UpdatedAt, d => d.Ignore())
                    .ForMember(d => d.UpdatedBy, d => d.Ignore())
                    .ForMember(d => d.DeletedAt, d => d.Ignore())
                    .ForMember(d => d.DeletedBy, d => d.Ignore())
                    ;
            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}