using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;
using Microsoft.Extensions.Logging;
using GroupAuthorizationServiceBase = Kebormed.Core.OrganizationService.Grpc.Generated.GroupAuthorizationService.GroupAuthorizationServiceBase;
using GroupAuthorizationService = Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.GroupAuthorizationService;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class GroupAuthorizationController : GroupAuthorizationServiceBase
    {
        private readonly GroupAuthorizationService service;
        private readonly IMapper mapper;
        private readonly ILogger<GroupAuthorizationController> logger;

        public GroupAuthorizationController(GroupAuthorizationService service,
            GroupAuthorizationGrpcMapper mapper,
            ILogger<GroupAuthorizationController> logger)
        {
            this.service = service;
            this.mapper = mapper.Mapper;
            this.logger = logger;
        }

        public override Task<QueryGroupAuthorization.Types.Response> QueryGroupAuthorization(QueryGroupAuthorization.Types.Request request, ServerCallContext context)
        {
            var command = this.mapper.Map<QueryGroupAuthorizationCriteria>(request);
            var result = this.service.QueryGroupAuthorization(command);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new QueryGroupAuthorization.Types.Response()
            {
                RequestOrganizationUserId = result.Value.RequestOrganizationUserId,
                OrganizationId = result.Value.OrganizationId,
                Permissions = { mapper.Map<IEnumerable<QueryGroupAuthorization.Types.Permission>>(result.Value.Permissions) }
            });
        }


        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<InvalidQueryGroupAuthorizationDataError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<InvalidQueryGroupAuthorizationDataError>().GenerateErrorDetail());
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }
    }
}
