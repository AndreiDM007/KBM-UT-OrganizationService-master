using System;
using System.Linq;
using AutoMapper;
using Kebormed.Core.OrganizationService.Web.Business.Services.UserInvitation.Models;
using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Mappers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kebormed.Core.OrganizationService.Web.Data.Repositories
{
    public class UserInvitationRepository : IUserInvitationRepository
    {
        private readonly OrganizationServiceDataContext context;
        private readonly IMapper mapper;

        public UserInvitationRepository(
            OrganizationServiceDataContext context,
            UserInvitationDataMapper userInvitationDataMapper)
        {
            this.context = context;
            mapper = userInvitationDataMapper.Mapper;
        }

        /// <inheritdoc />
        public bool UserInvitationExists(string invitationGuid, string externalUserId)
        {
            return context
                .UserInvitations
                .Any(ui => ui.InvitationGuid.Equals(invitationGuid, StringComparison.InvariantCultureIgnoreCase) &&
                           ui.ExternalUserId.Equals(externalUserId, StringComparison.InvariantCultureIgnoreCase));
        }

        public GetUserInvitationResult GetUserInvitation(string invitationGuid, string externalUserId)
        {
            var entity = GetValidUserInvitations()
                .FirstOrDefault(
                    ui => ui.InvitationGuid.Equals(invitationGuid, StringComparison.InvariantCultureIgnoreCase) &&
                          ui.ExternalUserId.Equals(externalUserId, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null) return null;

            return mapper.Map<GetUserInvitationResult>(entity);
        }

        /// <inheritdoc />
        public int CreateUserInvitation(CreateUserInvitationCommand command, string inviteGuid)
        {
            var entity = mapper.Map<UserInvitationEntity>(command);
            entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            entity.InvitationGuid = inviteGuid;

            context.UserInvitations.Add(entity);
            context.SaveChanges();
            return entity.UserInvitationEntityId;
        }

        /// <inheritdoc />
        public void UpdateUserInvitation(UpdateUserInvitationCommand command)
        {
            var entity = GetValidUserInvitations()
                .FirstOrDefault(
                    ui => ui.InvitationGuid.Equals(command.InvitationGuid, StringComparison.InvariantCultureIgnoreCase) &&
                          ui.ExternalUserId.Equals(command.ExternalUserId, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null) return;
            
            if (command.AcceptedAt.HasValue)
                entity.AcceptedAt = command.AcceptedAt.Value;
            else if (command.DeclinedAt.HasValue)
                entity.DeclinedAt = command.DeclinedAt.Value;

            context.SaveChanges();
        }

        /// <inheritdoc />
        public void DeleteUserInvitation(string invitationGuid, string externalUserId)
        {
            var entity = GetValidUserInvitations()
                .First(ui => string
                    .Equals(ui.InvitationGuid, invitationGuid, StringComparison.InvariantCultureIgnoreCase));

            if (entity == null) return;

            entity.DeletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            context.SaveChanges();
        }

        private IQueryable<UserInvitationEntity> GetValidUserInvitations()
        {
            return context
                .UserInvitations
                .Where(i => i.DeletedAt == null &&
                            i.AcceptedAt == null && //ignore invitations already answered
                            i.DeclinedAt == null);
        }
    }
}