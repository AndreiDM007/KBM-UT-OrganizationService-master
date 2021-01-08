using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Errors;
using Kebormed.Core.OrganizationService.Web.Business.Services.Organization.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Organization
{
    public class OrganizationService
    {
        private readonly IOrganizationRepository organizationRepository;

        public OrganizationService(
            IOrganizationRepository organizationRepository
        )
        {
            this.organizationRepository = organizationRepository;
        }

        /// <summary>
        /// Creates organization
        /// </summary>
        /// <param name="createOrganizationCommand">Model for all needed fields to create a tenant</param>
        /// <returns>ID of created Organization</returns>
        /// <exception cref="InvalidCreateOrganizationDataError">If create data is not valid</exception>
        /// <exception cref="OrganizationAlreadyExistsError">If organization already exists</exception>
        public Result<int> CreateOrganization(CreateOrganizationCommand createOrganizationCommand)
        {
            if (createOrganizationCommand?.Name == null || createOrganizationCommand.Name.Length == 0)
            {
                return new Result<int>(OrganizationServiceErrors.InvalidCreateOrganizationDataError(nameof(createOrganizationCommand.Name)));
            }
            if (this.organizationRepository.OrganizationExistsByName(createOrganizationCommand.Name))
            {
                return new Result<int>(OrganizationServiceErrors.OrganizationAlreadyExistsError());
            }
            return new Result<int>(this.organizationRepository.CreateOrganization(createOrganizationCommand));
        }

        public Result<GetOrganizationResult> GetOrganization(int organizationId)
        {
            if (organizationId <= 0)
            {
                return new Result<GetOrganizationResult>(OrganizationServiceErrors.InvalidOrganizationIdError());
            }

            if (!organizationRepository.OrganizationExists(organizationId))
            {
                return new Result<GetOrganizationResult>(OrganizationServiceErrors.OrganizationNotFoundError());
            }

            var res = this.organizationRepository.GetOrganization(organizationId);

            return new Result<GetOrganizationResult>(res);
        }

        public EmptyResult DeleteOrganization(int organizationId)
        {
            if (organizationId <= 0)
            {
                return new EmptyResult(OrganizationServiceErrors.InvalidOrganizationIdError());
            }

            if (!organizationRepository.OrganizationExists(organizationId))
            {
                return new EmptyResult(OrganizationServiceErrors.OrganizationNotFoundError());
            }

            this.organizationRepository.DeleteOrganization(organizationId);

            return new EmptyResult();
        }

        public Result<int> QueryOrganization(string externalUserId)
        {
            if (string.IsNullOrWhiteSpace(externalUserId))
            {
                return new Result<int>(OrganizationServiceErrors.InvalidExternalUserId());
            }
            var result = organizationRepository.FindOrganizationIdByExternalUserId(externalUserId);

            if (result is null)
            {
                return new Result<int>(OrganizationServiceErrors.InvalidExternalUserId());
            }

            return new Result<int>(result.Value);
        }

        /// <summary>
        /// Updates an organization with passed fields
        /// </summary>
        /// <param name="command">contains ID and fields</param>
        /// <returns></returns>
        public EmptyResult UpdateOrganization(UpdateOrganizationCommand command)
        {
            if (command?.OrganizationId == null || command.OrganizationId <= 0)
            {
                return new EmptyResult(OrganizationUserServiceErrors.InvalidOrganizationId());
            }
            if (!organizationRepository.OrganizationExists(command.OrganizationId.Value))
            {
                return new EmptyResult(OrganizationServiceErrors.OrganizationNotFoundError());
            }
            if (command.Name != null && organizationRepository.OrganizationExistsByName(command.Name, command.OrganizationId.Value))
            {
                return new EmptyResult(OrganizationServiceErrors.InvalidUpdateOrganizationNameError(nameof(command.Name)));
            }
            organizationRepository.UpdateOrganization(command);
            
            return new EmptyResult();
        }

        public Result<bool> ExistOrganization(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidOrganizationName());
            }

            return new Result<bool>(organizationRepository.OrganizationExistsByName(name));
        }

        public Result<bool> ExistsOrganizationById(int organizationId)
        {
            if (organizationId <= 0)
            {
                return new Result<bool>(OrganizationUserServiceErrors.InvalidOrganizationId());
            }

            return new Result<bool>(organizationRepository.OrganizationExists(organizationId));
        }
    }
}