﻿using System.Linq;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Kebormed.Core.Communication.Grpc.Extensions;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Mappers
{
    public class GroupAuthorizationGrpcMapper
    {
        public IMapper Mapper;

        public GroupAuthorizationGrpcMapper()
        {
            var automapConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Int32Value, int?>().ConvertUsing(i => i.GetValue());
                config.CreateMap<Int64Value, long?>().ConvertUsing(i => i.GetValue());
                config.CreateMap<DoubleValue, double?>().ConvertUsing(i => i.GetValue());
                config.CreateMap<FloatValue, float?>().ConvertUsing(i => i.GetValue());
                config.CreateMap<StringValue, string>().ConvertUsing(i => i.GetValue());
                config.CreateMap<BoolValue, bool>().ConvertUsing(i => i.GetValue().GetValueOrDefault());

                //outgoing
                config.CreateMap<string, StringValue>().ConvertUsing(i => i == null ? null : new StringValue { Value = i });
                config.CreateMap<float?, FloatValue>().ConvertUsing(i => i.HasValue ? new FloatValue { Value = i.Value } : null);
                config.CreateMap<float, FloatValue>().ConvertUsing(i => new FloatValue { Value = i });
                config.CreateMap<double?, DoubleValue>().ConvertUsing(i => i.HasValue ? new DoubleValue { Value = i.Value } : null);
                config.CreateMap<double, DoubleValue>().ConvertUsing(i => new DoubleValue { Value = i });
                config.CreateMap<long?, Int64Value>().ConvertUsing(i => i.HasValue ? new Int64Value { Value = i.Value } : null);
                config.CreateMap<long, Int64Value>().ConvertUsing(i => new Int64Value { Value = i });
                config.CreateMap<int?, Int32Value>().ConvertUsing(i => i.HasValue ? new Int32Value { Value = i.Value } : null);
                config.CreateMap<int, Int32Value>().ConvertUsing(i => new Int32Value { Value = i });
                config.CreateMap<bool?, BoolValue>().ConvertUsing(i => i.HasValue ? new BoolValue { Value = i.Value } : null);
                config.CreateMap<bool, BoolValue>().ConvertUsing(i => new BoolValue { Value = i });

                
                config.CreateMap<GroupAuthorizationPermission, QueryGroupAuthorization.Types.Permission>();
                config.CreateMap<QueryGroupAuthorizationResult, QueryGroupAuthorization.Types.Response>();
                config.CreateMap<QueryGroupAuthorization.Types.Request, QueryGroupAuthorizationCriteria>();


            });

            automapConfig.AssertConfigurationIsValid();
            this.Mapper = automapConfig.CreateMapper();
        }
    }
}
