using System;
using System.Collections.Generic;
using System.Linq;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group;
using Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models;

using Kebormed.Core.OrganizationService.Web.Data.Repositories;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization
{
    public class GroupAuthorizationService
    {
        private readonly IGroupAuthorizationRepository groupAuthorizationRepository;
        private readonly IOrganizationUserRepository organizationUserRepository;

        public GroupAuthorizationService(IGroupAuthorizationRepository groupAuthorizationRepository,
            IOrganizationUserRepository organizationUserRepository)
        {
            this.groupAuthorizationRepository = groupAuthorizationRepository;
            this.organizationUserRepository = organizationUserRepository;
        }

        public Result<QueryGroupAuthorizationResult> QueryGroupAuthorization(QueryGroupAuthorizationCriteria command)
        {
            if (!command.OrganizationId.HasValue)
            {
                return new Result<QueryGroupAuthorizationResult>(GroupAuthorizationServiceErrors.InvalidQueryGroupAuthorizationDataError(nameof(command.OrganizationId)));
            }

            if (!command.RequestOrganizationUserId.HasValue)
            {
                return new Result<QueryGroupAuthorizationResult>(GroupAuthorizationServiceErrors.InvalidQueryGroupAuthorizationDataError(nameof(command.RequestOrganizationUserId)));
            }

            if (!organizationUserRepository.OrganizationUserExists(command.RequestOrganizationUserId.Value, command.OrganizationId.Value))
            {
                return new Result<QueryGroupAuthorizationResult>(GroupAuthorizationServiceErrors.InvalidQueryGroupAuthorizationDataError(nameof(command.RequestOrganizationUserId)));
            }

            var permissionsForRequestedUser = groupAuthorizationRepository.QueryGroupAuthorization(command);
            List<GroupAuthorizationPermission> permissionsResult;
            // this way we can support fetching all permitted users, when no target list is sent
            if (command.TargetOrganizationUserIdCollection != null && command.TargetOrganizationUserIdCollection.Any())
            {

                //if all targets are the same as the request, returns allowed
                if (command.TargetOrganizationUserIdCollection.All(tou => tou == command.RequestOrganizationUserId))
                {
                    permissionsResult = command.TargetOrganizationUserIdCollection.Select(tou =>
                        new GroupAuthorizationPermission
                        {
                            TargetOrganizationUserId = tou,
                            Allowed = true
                        }).ToList();
                }
                else
                {
                    permissionsResult = new List<GroupAuthorizationPermission>();
                    foreach (var organizationUserId in command.TargetOrganizationUserIdCollection)
                    {
                        permissionsResult.Add(new GroupAuthorizationPermission
                        {
                            TargetOrganizationUserId = organizationUserId,
                            Allowed = permissionsForRequestedUser.Any(ou => ou == organizationUserId)
                        });
                    }
                }
            }
            else
            {
                //send all
                permissionsResult = permissionsForRequestedUser.Select(ou => new GroupAuthorizationPermission
                {
                    TargetOrganizationUserId = ou,
                    Allowed = true
                }).ToList();

            }

            var result = new QueryGroupAuthorizationResult
            {
                RequestOrganizationUserId = command.RequestOrganizationUserId.Value,
                OrganizationId = command.OrganizationId.Value,
                Permissions = permissionsResult
            };

            return new Result<QueryGroupAuthorizationResult>(result);
        }


    }
}
