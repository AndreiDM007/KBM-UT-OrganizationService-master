using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.Common.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models;
using Kebormed.Core.OrganizationService.Web.Business.Services.GroupAuthorization.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class GroupAuthorizationRepository : IGroupAuthorizationRepository
    {
        #region Members

        private readonly OrganizationServiceDataContext context;
        
        #endregion

        public GroupAuthorizationRepository(OrganizationServiceDataContext context)
        {
            this.context = context;
        }
        
        public List<int> QueryGroupAuthorization(QueryGroupAuthorizationCriteria command)
        {
            const string query = @"
            DECLARE @RequestOrganizationUserGroups TABLE
            (
              GroupId int
            )

            DECLARE @AllowedOrganizationUserIds TABLE
            (
              OrganizationUserEntityId int
            )

            -- #1 - Identify the groups that belong to the Requested OrganizationUserId
            INSERT INTO @RequestOrganizationUserGroups (GroupId)
            SELECT
	            g.GroupEntityId
            FROM 
	            [dbo].[GroupMember] gm
	            INNER JOIN dbo.[Group] g ON gm.GroupId = g.GroupEntityId AND g.DeletedAt IS NULL
            WHERE 
	            gm.OrganizationId = @OrganizationId
	            AND gm.DeletedAt	IS NULL
	            AND gm.OrganizationUserId =  @RequestOrganizationUserId

            --SELECT * FROM @RequestOrganizationUserGroups roug

            --#2 - Identify all the Children (if any) from the Parent Group
            ;WITH cte AS 
            (
            SELECT 
	            g.GroupEntityId
            FROM 
	            dbo.[Group] g 
	            INNER JOIN  @RequestOrganizationUserGroups roug ON roug.GroupId = g.GroupEntityId
            UNION ALL
            SELECT 
	            g.GroupEntityId
            FROM 
	            dbo.[Group] g 
	            JOIN cte ON g.ParentGroupId = cte.GroupEntityId
            )

            --#3 - Get all the OrganizationUserIds that belong to children group
            INSERT INTO @AllowedOrganizationUserIds (OrganizationUserEntityId)
            SELECT OrganizationUserId
                FROM cte
	            INNER JOIN [dbo].[GroupMember] gm ON gm.GroupId = cte.GroupEntityId
				WHERE gm.DeletedAt IS NULL


            --SELECT * FROM @AllowedOrganizationUserIds



            --#4 Find OrganizationUserIds that have no GROUP associated
            INSERT INTO @AllowedOrganizationUserIds (OrganizationUserEntityId)
            SELECT  
	            ou.OrganizationUserEntityId  
            FROM 
	            dbo.OrganizationUser ou
	            LEFT JOIN [dbo].[GroupMember] gm 
		            ON ou.OrganizationUserEntityId = gm.OrganizationUserId 
		            AND ou.OrganizationId = gm.OrganizationId
            WHERE
	            ou.OrganizationId = @OrganizationId
	            AND ou.DeletedAt IS NULL 
	            AND ou.RollbackedAt IS NULL
	            AND 
				 (
					(gm.OrganizationUserId IS NULL) 
					OR 
                    --selects users that were in groups but were deleted
					(gm.DeletedAt IS NOT NULL AND gm.OrganizationUserId NOT IN (SELECT OrganizationUserId From [dbo].[GroupMember] WHERE DeletedAt IS NULL))
				 )

            --#5 return data in such a way that can be consumed by EF core (name of the column)
            SELECT DISTINCT
              ou.*
            FROM dbo.OrganizationUser ou
            INNER JOIN @AllowedOrganizationUserIds aoui
              ON aoui.OrganizationUserEntityId = ou.OrganizationUserEntityId
            WHERE ou.OrganizationId = @OrganizationId   
            ";

            //TODO: this last block (#5) of code was replaced by the directly selecting the @AllowedOrganizationUserIds table because it already contains group users + users that are not in any group;
            /*
             *     SELECT 
	            gm.* 
            FROM 
	            dbo.GroupMember gm 
	            INNER JOIN @AllowedOrganizationUserIds aoui 
		            ON aoui.OrganizationUserId = gm.OrganizationUserId
            WHERE 
	            gm.OrganizationId = @OrganizationId
             */


            var organizationId = new SqlParameter("@OrganizationId", command.OrganizationId);
            var requestOrganizationUserId = new SqlParameter("@RequestOrganizationUserId", command.RequestOrganizationUserId);
            return context.
                OrganizationUsers
                .FromSql(query, organizationId, requestOrganizationUserId)
                .AsNoTracking()
                .Select(b =>
                    b.OrganizationUserEntityId)
                .ToList();
            
        }
    }
}
