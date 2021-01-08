using AutoMapper;
using Google.Protobuf.Collections;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser.Models;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Mappers
{
    public class OrganizationUserGrpcMapper
    {
        public IMapper Mapper { get; }

        public OrganizationUserGrpcMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<QueryOrganizationUser.Types.Request, QueryOrganizationUserCriteria>()
                    .ForMember(d => d.OrganizationIds, d => d.MapFrom(src => src.OrganizationIdList))
                    .ForMember(d => d.UserType, d => d.MapFrom(src => src.UserType))
                    ;
                
                config
                    .CreateMap<CreateOrganizationUser.Types.Request, CreateOrganizationUserCommand>()
                    ;
                
                config.CreateMap<UpdateOrganizationUser.Types.Request, UpdateOrganizationUserCommand>();
                config.CreateMap<ProfileValuesListModel, GetOrganizationUser.Types.ProfileValue>();
                config.CreateMap<AssociationOrganizationUserListModel, GetOrganizationUser.Types.AssociatedOrganizationUser>();
                config.CreateMap<OrganizationUserGroupListModel, GetOrganizationUser.Types.UserGroup>();
                config.CreateMap<CreateOrganizationUsersAssociation.Types.Request, CreateOrganizationUsersAssociationCommand>();
                config.CreateMap<UpdateOrganizationUsersAssociation.Types.Request, UpdateOrganizationUsersAssociationCommand>();                

                config.CreateMap<ProfileValuesListModel, QueryOrganizationUser.Types.ProfileValues>();

                config.CreateMap<GetOrganizationAdminResult, GetOrganizationAdmin.Types.Response>();
                
                config.CreateMap<SingleOrganizationOrgUserListModel, GetSingleOrganizationOrgUsers.Types.SingleOrganizationOrgUserListView>();

                bool IsToRepeatedField(PropertyMap pm)
                {
                    if (pm.DestinationType.IsConstructedGenericType)
                    {
                        var destGenericBase = pm.DestinationType.GetGenericTypeDefinition();
                        return destGenericBase == typeof(RepeatedField<>);
                    }
                    return false;
                }
                config.ForAllPropertyMaps(IsToRepeatedField, (propertyMap, opts) => opts.UseDestinationValue());

            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}