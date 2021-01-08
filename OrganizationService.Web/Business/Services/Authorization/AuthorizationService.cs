using System;
using System.Collections.Generic;
using System.Linq;
using Kebormed.Core.Business.Results;
using Kebormed.Core.OrganizationService.Web.Business.Services.Authorization.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.OrganizationUser;
using Kebormed.Core.OrganizationService.Web.Data.Repositories;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Authorization
{
    public class AuthorizationService
    {
        private readonly IRolePermissionRepository rolePermissionRepository;
        private readonly IOrganizationUserPermissionRepository organizationUserPermissionRepository;
        private readonly IOrganizationUserRoleRepository organizationUserRoleRepository;        

        public AuthorizationService(IRolePermissionRepository rolePermissionRepository,
            IOrganizationUserPermissionRepository organizationUserPermissionRepository,
            IOrganizationUserRoleRepository organizationUserRoleRepository
        )
        {
            this.rolePermissionRepository = rolePermissionRepository;
            this.organizationUserPermissionRepository = organizationUserPermissionRepository;
            this.organizationUserRoleRepository = organizationUserRoleRepository;            
        }
                      
        /// <summary>
        /// Creates OrganizationUserRole
        /// </summary>
        /// <param name="createOrganizationUserRoleCommand">Model for all needed fields to create a tenant</param>
        /// <returns>ID of created OrganizationUserRole</returns>
        /// <exception cref="InvalidCreateOrganizationUserRoleDataError">If create data is not valid</exception>
        /// <exception cref="OrganizationUserRoleAlreadyExistsError">If OrganizationUserRole already exists</exception>
        public Result<int> CreateOrganizationUserRole(CreateOrganizationUserRoleCommand command)
        {
            if (!command.OrganizationUserId.HasValue)
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.OrganizationUserId)));
            }

            if (!command.OrganizationId.HasValue)
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.OrganizationId)));
            }

            if (string.IsNullOrEmpty(command.RoleId))
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.RoleId)));
            }

            var exists = this.organizationUserRoleRepository.ExistsOrganizationUserRole(command.OrganizationUserId.Value, command.OrganizationId.Value, command.RoleId);
            if (exists)
            {
                return new Result<int>(AuthorizationServiceErrors.EntityAlreadyExistsError(nameof(CreateOrganizationUserRoleCommand)));
            }

            var id = this.organizationUserRoleRepository.CreateOrganizationUserRole(command);

            return new Result<int>(id);

        }

        public EmptyResult DeleteOrganizationUserRole(DeleteOrganizationUserRoleCommand command)
        {
            if (string.IsNullOrEmpty(command.RoleId))
            {
                return new Result<bool>(AuthorizationServiceErrors.InvalidDeleteEntityError(nameof(command.RoleId)));
            }

            if (!command.OrganizationUserId.HasValue)
            {
                return new Result<bool>(AuthorizationServiceErrors.InvalidDeleteEntityError(nameof(command.OrganizationUserId)));
            }

            if (!command.OrganizationId.HasValue)
            {
                return new Result<bool>(AuthorizationServiceErrors.InvalidDeleteEntityError(nameof(command.OrganizationId)));
            }

            organizationUserRoleRepository.DeleteOrganizationUserRole(command);

            return new Result<EmptyResult>();
        }       

        /// <summary>
        /// Updates an OrganizationUserPermission with passed fields
        /// </summary>
        /// <param name="command">contains ID and fields</param>
        /// <returns></returns>
        public EmptyResult UpdateOrganizationUserPermission(UpdateOrganizationUserPermissionCommand command)
        {
            if (!command.OrganizationUserId.HasValue)
            {
                return new EmptyResult(AuthorizationServiceErrors.InvalidUpdateOrganizationNameError(nameof(command.OrganizationUserId)));
            }

            if (string.IsNullOrEmpty(command.UserId))
            {
                return new EmptyResult(AuthorizationServiceErrors.InvalidExternalUserId());
            }

            if (!organizationUserPermissionRepository.ExistsOrganizationUserPermission(command.OrganizationUserId.Value, command.UserId))
            {
                organizationUserPermissionRepository.CreateOrganizationUserPermission(
                    command.OrganizationUserId.Value,
                    command.UserId,
                    command.Permissions);
            }
            else
            {
                organizationUserPermissionRepository.UpdateOrganizationUserPermission(command);
            }

            return new EmptyResult();
        }

        public Result<bool> ExistRolePermissionById(string roleId, int? organizationId)
        {
            if (!organizationId.HasValue)
            {
                return new Result<bool>(AuthorizationServiceErrors.InvalidOrganizationIdError());
            }           

            var result = rolePermissionRepository.ExistsRolePermission(organizationId.Value, roleId);
            return new Result<bool>(result);
        }
        
        /// <summary>
        /// Creates RolePermission
        /// </summary>
        /// <param name="createRolePermissionCommand">Model for all needed fields to create a tenant</param>
        /// <returns>ID of created RolePermission</returns>
        /// <exception cref="InvalidCreateRolePermissionDataError">If create data is not valid</exception>
        /// <exception cref="RolePermissionAlreadyExistsError">If RolePermission already exists</exception>
        public Result<int> CreateRolePermission(CreateRolePermissionCommand command)
        {
            if (!command.OrganizationId.HasValue)
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.OrganizationId)));
            }

            if (string.IsNullOrEmpty(command.RoleId))
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.RoleId)));
            }

            if (string.IsNullOrEmpty(command.RoleName))
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.RoleName)));
            }

            if (string.IsNullOrEmpty(command.Permissions))
            {
                return new Result<int>(AuthorizationServiceErrors.InvalidCreateOrganizationDataError(nameof(command.Permissions)));
            }

            var exists = this.rolePermissionRepository.ExistsRolePermission(command.OrganizationId.Value, command.RoleId);
            if (exists)
            {
                return new Result<int>(AuthorizationServiceErrors.EntityAlreadyExistsError(nameof(CreateRolePermissionCommand)));
            }

            return new Result<int>(this.rolePermissionRepository.CreateRolePermission(command));
        }
        
        /// <summary>
        /// Updates an RolePermission with passed fields
        /// </summary>
        /// <param name="command">contains ID and fields</param>
        /// <returns></returns>
        public EmptyResult UpdateRolePermission(UpdateRolePermissionCommand command)
        {
            if (!command.OrganizationId.HasValue)
            {
                return new EmptyResult(AuthorizationServiceErrors.InvalidUpdateOrganizationNameError(nameof(command.OrganizationId)));
            }

            if (string.IsNullOrEmpty(command.RoleId))
            {
                return new EmptyResult(AuthorizationServiceErrors.InvalidUpdateOrganizationNameError(nameof(command.RoleId)));
            }

            if (!rolePermissionRepository.ExistsRolePermission(command.OrganizationId.Value, command.RoleId))
            {
                return new EmptyResult(AuthorizationServiceErrors.InvalidUpdateOrganizationNameError(nameof(command.OrganizationId)));
            }

            rolePermissionRepository.UpdateRolePermission(command);

            return new EmptyResult();
        }       
        
        public EmptyResult DeleteRolePermission(DeleteRolePermissionCommand command)
        {
            if (string.IsNullOrEmpty(command.RoleId))
            {
                return new Result<bool>(AuthorizationServiceErrors.InvalidDeleteEntityError(nameof(command.RoleId)));
            }

            if (!command.OrganizationId.HasValue)
            {
                return new Result<bool>(AuthorizationServiceErrors.InvalidDeleteEntityError(nameof(command.OrganizationId)));
            }

            rolePermissionRepository.DeleteRolePermission(command);
            return new Result<bool>();
        }

        public Result<QueryOrganizationUserRoleResult> QueryOrganizationUserRole(QueryOrganizationUserRoleCriteria criteria)
        {
            HandlePaginationAndDirectionValidation(criteria);
            var result = organizationUserRoleRepository.QueryOrganizationUserRole(criteria);

            return new Result<QueryOrganizationUserRoleResult>(result);
        }

        public Result<QueryOrganizationUserPermissionResult> QueryOrganizationUserPermission(QueryOrganizationUserPermissionCriteria criteria)
        {
            HandlePaginationAndDirectionValidation(criteria);
            var result = organizationUserPermissionRepository.QueryOrganizationUserPermission(criteria);


            return new Result<QueryOrganizationUserPermissionResult>(result);
        }

        public Result<QueryRolePermissionResult> QueryRolePermission(QueryRolePermissionCriteria criteria)
        {
            HandlePaginationAndDirectionValidation(criteria);

            if (!RolePermissionRepository.SortableFields.Contains(criteria.OrderBy, StringComparer.OrdinalIgnoreCase))
            {
                criteria.OrderBy = string.Empty;
            }

            var result = rolePermissionRepository.QueryRolePermission(criteria);
            return new Result<QueryRolePermissionResult>(result);
        }

        public Result<OrganizationUserAndRolePermissionsListView> QueryOrganizationUserAndRolePermission(QueryOrganizationUserAndRolePermissionCriteria criteria)
        {
            if (string.IsNullOrEmpty(criteria.UserId))
            {
                return new Result<OrganizationUserAndRolePermissionsListView>(AuthorizationServiceErrors.InvalidExternalUserId());
            }

            if (!criteria.OrganizationId.HasValue)
            {
                return new Result<OrganizationUserAndRolePermissionsListView>(AuthorizationServiceErrors.InvalidOrganizationIdError());
            }
            var userPermissions = organizationUserPermissionRepository.QueryAllOrganizationUserPermission(criteria.OrganizationId.Value, criteria.UserId);
            var rolePermissions = rolePermissionRepository.QueryRolePermissionByExternalUserId(criteria.OrganizationId.Value,
                    criteria.UserId);
            var result = new OrganizationUserAndRolePermissionsListView
            {
                OrganizationId = criteria.OrganizationId,
                OrganizationUserId = rolePermissions.FirstOrDefault(rp => rp.OrganizationUserId.HasValue)
                                         ?.OrganizationUserId ??
                                     userPermissions.FirstOrDefault(up => up.OrganizationUserId.HasValue)
                                         ?.OrganizationUserId,
                UserId = criteria.UserId,
                Permissions = new List<string>()
            };

            if (userPermissions != null && userPermissions.Any())
            {
                result.Permissions.AddRange(userPermissions.Select(up => up.Permissions));
            }
            if (rolePermissions != null && rolePermissions.Any())
            {
                result.Permissions.AddRange(rolePermissions.Select(up => up.Permissions));
            }
            
            return new Result<OrganizationUserAndRolePermissionsListView>(result);
        }

        private static void HandlePaginationAndDirectionValidation(IQueryCriteria criteria)
        {
            if (criteria.Page == null || criteria.Page.Value <= 0)
            {
                criteria.Page = 1;
            }

            if (criteria.PageSize == null || criteria.PageSize.Value <= 0)
            {
                criteria.PageSize = 10;
            }


            if (string.IsNullOrWhiteSpace(criteria.Direction))
            {
                criteria.Direction = "ASC";
            }
            else
            {
                if (!criteria.Direction.ToUpper().Equals("ASC") && !criteria.Direction.ToUpper().Equals("DESC"))
                {
                    criteria.Direction = "ASC";
                }
            }
        }
    }
}