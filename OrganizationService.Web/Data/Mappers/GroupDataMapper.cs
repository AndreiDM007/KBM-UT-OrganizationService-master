using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Mappers
{
    public class GroupDataMapper
    {
        public IMapper Mapper;

        public GroupDataMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CreateGroupCommand, GroupEntity>()
                    .ForMember(dest => dest.GroupEntityId, dest => dest.Ignore())
                    .ForMember(dest => dest.ParentGroup, dest => dest.Ignore())
                    .ForMember(dest => dest.SubGroups, dest => dest.Ignore())
                    .ForMember(dest => dest.GroupMembers, dest => dest.Ignore())
                    .ForMember(dest => dest.CreatedAt, dest => dest.Ignore())
                    .ForMember(dest => dest.UpdatedAt, dest => dest.Ignore())
                    .ForMember(dest => dest.DeletedAt, dest => dest.Ignore())                    
                    ;

                config.CreateMap<UpdateGroupCommand, GroupEntity>()
                    .ForMember(dest => dest.GroupEntityId, dest => dest.Ignore())
                    .ForMember(dest => dest.ParentGroup, dest => dest.Ignore())
                    .ForMember(dest => dest.SubGroups, dest => dest.Ignore())
                    .ForMember(dest => dest.GroupMembers, dest => dest.Ignore())
                    .ForMember(dest => dest.CreatedAt, dest => dest.Ignore())
                    .ForMember(dest => dest.UpdatedAt, dest => dest.Ignore())
                    .ForMember(dest => dest.DeletedAt, dest => dest.Ignore())
                    ;

                config.CreateMap<AssociateOrganizationUserToGroupCommand, GroupMemberEntity>()
                    .ForMember(dest => dest.GroupMemberEntityId, dest => dest.Ignore())
                    .ForMember(dest => dest.Group, dest => dest.Ignore())
                    .ForMember(dest => dest.Member, dest => dest.Ignore())
                    .ForMember(dest => dest.CreatedAt, dest => dest.Ignore())
                    .ForMember(dest => dest.UpdatedAt, dest => dest.Ignore())
                    .ForMember(dest => dest.DeletedAt, dest => dest.Ignore())
                    ;

                config
                    .CreateMap<GroupMemberEntity, GroupMemberListModel>()
                    .ForMember(d => d.Id, o => o.MapFrom(src => src.GroupMemberEntityId))
                    .ForMember(d => d.UserName, o => o.MapFrom(src => src.Member.Username))
                    .ForMember(d => d.FirstName, o => o.MapFrom(src => src.Member.FirstName))
                    .ForMember(d => d.LastName, o => o.MapFrom(src => src.Member.LastName))
                    .ForMember(d => d.CreationTime, o => o.MapFrom(src => src.CreatedAt))
                    .ForMember(d => d.UserType, o => o.MapFrom(src => src.Member.UserType))
                    .ForMember(d => d.OrganizationUserId, o => o.MapFrom(src => src.Member.OrganizationUserEntityId))
                    ;

                config
                    .CreateMap<GroupEntity, GroupListModel>()
                    .ForMember(d => d.GroupId, o => o.MapFrom(src => src.GroupEntityId))
                    ;

                config
                    .CreateMap<GroupMemberEntity, MemberGroupListModel>()
                    .ForMember(d => d.GroupName, o => o.MapFrom(src => src.Group.Name))
                    ;


            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}
