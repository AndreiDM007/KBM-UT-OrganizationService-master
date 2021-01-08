using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Grpc.Generated;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models;
using Kebormed.Core.OrganizationService.Web.Grpc.Mappers;
using Microsoft.Extensions.Logging;

namespace Kebormed.Core.OrganizationService.Web.Grpc.Controllers
{
    public class OrganizationController : OrganizationService.Grpc.Generated.OrganizationService.OrganizationServiceBase
    {
        private readonly Business.Services.Organization.OrganizationService organizationService;
        private readonly IMapper mapper;
        private readonly ILogger<OrganizationController> logger;

        public OrganizationController(
            Business.Services.Organization.OrganizationService organizationService,
            OrganizationGrpcMapper organizationGrpcMapper,
            ILogger<OrganizationController> logger)
        {
            this.organizationService = organizationService;
            this.mapper = organizationGrpcMapper.Mapper;
            this.logger = logger;
        }

        public override Task<CreateOrganization.Types.Response> CreateOrganization(CreateOrganization.Types.Request request, ServerCallContext context)
        {
            var model = this.mapper.Map<CreateOrganizationCommand>(request);
            var result = this.organizationService.CreateOrganization(model);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new CreateOrganization.Types.Response
            {
                OrganizationId = result.Value
            });
        }

        public override Task<DeleteOrganization.Types.Response> DeleteOrganization(DeleteOrganization.Types.Request request, ServerCallContext context)
        {
            var organizationId = request.OrganizationId;
            var result = this.organizationService.DeleteOrganization(organizationId);

            if (result.IsFailure)
            {
                logger.LogError(result.Errors.First().Code);
                HandleResultFailure(context, result);
                return null;
            }

            return Task.FromResult(new DeleteOrganization.Types.Response());
        }

        public override Task<QueryOrganization.Types.Response> QueryOrganization(QueryOrganization.Types.Request request, ServerCallContext context)
        {
            var result = this.organizationService.QueryOrganization(request.ExternalUserId);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            var response = new QueryOrganization.Types.Response
            {
                OrganizationId = result.Value
            };
            return Task.FromResult(response);
        }

        public override Task<GetOrganization.Types.Response> GetOrganization(GetOrganization.Types.Request request,
            ServerCallContext context)
        {
            var result = organizationService.GetOrganization(request.OrganizationId);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }

            var response = new GetOrganization.Types.Response
            {
                OrganizationId = result.Value.OrganizationId,
                Name = result.Value.Name,
                IsActive = result.Value.IsActive
            };

            return Task.FromResult(response);
        }

        public override Task<UpdateOrganization.Types.Response> UpdateOrganization(UpdateOrganization.Types.Request request, ServerCallContext context)
        {
            var command = mapper.Map<UpdateOrganizationCommand>(request);
            var result = organizationService.UpdateOrganization(command);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new UpdateOrganization.Types.Response());
        }

        public override Task<ExistOrganization.Types.Response> ExistOrganization(ExistOrganization.Types.Request request, ServerCallContext context)
        {
            
            var result = organizationService.ExistOrganization(request.Name);
            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new ExistOrganization.Types.Response()
            {
                Exists = result.Value
            });
        }

        public override Task<ExistsOrganizationById.Types.Response> ExistsOrganizationById(ExistsOrganizationById.Types.Request request, ServerCallContext context)
        {
            var result = organizationService.ExistsOrganizationById(request.OrganizationId);

            if (result.IsFailure)
            {
                HandleResultFailure(context, result);
                return null;
            }
            return Task.FromResult(new ExistsOrganizationById.Types.Response()
            {
                Exists = result.Value
            });
        }

        private static void HandleResultFailure<T>(ServerCallContext context, T result) where T : EmptyResult
        {
            if (result.HasError<OrganizationAlreadyExistsError>())
                context.Status = new Status(StatusCode.AlreadyExists, result.GetError<OrganizationAlreadyExistsError>().GenerateErrorDetail());
            else if (result.HasError<InvalidCreateOrganizationDataError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidCreateOrganizationDataError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationIdError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationIdError>().GenerateErrorDetail());
            else if (result.HasError<InvalidUpdateOrganizationNameError>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidUpdateOrganizationNameError>().GenerateErrorDetail());
            else if (result.HasError<OrganizationNotFoundError>())
                context.Status = new Status(StatusCode.NotFound, result.GetError<OrganizationNotFoundError>().GenerateErrorDetail());
            else if (result.HasError<InvalidOrganizationName>())
                context.Status = new Status(StatusCode.InvalidArgument, result.GetError<InvalidOrganizationName>().GenerateErrorDetail());
            else
                context.Status = new Status(StatusCode.Internal, string.Join(',', result.GetErrors().Select(e => e.GenerateErrorDetail())));
        }
    }
}