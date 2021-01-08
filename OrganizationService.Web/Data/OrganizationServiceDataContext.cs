using Kebormed.Core.OrganizationService.Web.Data.Entities;
using Kebormed.Core.OrganizationService.Web.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace Kebormed.Core.OrganizationService.Web.Data
{
    public partial class OrganizationServiceDataContext : DbContext
    {
        public OrganizationServiceDataContext(DbContextOptions<OrganizationServiceDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ProfileEntity> Profiles { get; set; }
        public virtual DbSet<ProfileParameterEntity> ProfileParameters { get; set; }
        public virtual DbSet<ProfileValueEntity> ProfileValues { get; set; }
        public virtual DbSet<OrganizationUserEntity> OrganizationUsers { get; set; }
        public virtual DbSet<OrganizationEntity> Organizations { get; set; }
        public virtual DbSet<AssociatedOrganizationUserEntity> AssociatedOrganizationUserEntities { get; set; }
        public virtual DbSet<GroupEntity> Groups { get; set; }
        public virtual DbSet<GroupMemberEntity> GroupMembers { get; set; }
        public virtual DbSet<OrganizationUserPermissionEntity> OrganizationUserPermissions { get; set; }
        public virtual DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public virtual DbSet<OrganizationUserRoleEntity> OrganizationUserRoles { get; set; }
        public virtual DbSet<UserInvitationEntity> UserInvitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<ProfileEntity>(entity =>
            {
                entity.ToTable("Profile");

                entity.HasIndex(e => e.OrganizationUserId);                              

                entity.HasOne(d => d.OrganizationUser)
                    .WithOne(p => p.Profile)
                    .HasForeignKey<ProfileEntity>(d => d.OrganizationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProfileParameterEntity>(entity =>
            {
                entity.ToTable("ProfileParameter");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ProfileValueEntity>(entity =>
            {
                entity.ToTable("ProfileValue");

                entity.HasIndex(e => e.ProfileId);

                entity.HasIndex(e => e.ProfileParameterId);

                entity.Property(e => e.Value).IsRequired();               

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ProfileValues)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ProfileParameter)
                    .WithMany(p => p.ProfileValues)
                    .HasForeignKey(d => d.ProfileParameterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OrganizationEntity>(entity =>
            {
                entity.ToTable("Organization");

                entity.HasKey(e => e.OrganizationEntityId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);     
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);                
            });

            modelBuilder.Entity<OrganizationUserEntity>(entity =>
            {
                entity.ToTable("OrganizationUser");

                entity.HasKey(e => e.OrganizationUserEntityId);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255); 
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255);  
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);  
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);  
                entity.Property(e => e.Email)
                    .HasMaxLength(255);  

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.OrganizationUsers)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity
                    .HasMany(d => d.AssociatedOrganizationUsers)
                    .WithOne(p => p.OrganizationUser)
                    .HasForeignKey(d => d.OrganizationUserId1)
                    .OnDelete(DeleteBehavior.Restrict)                    
                    ;
                entity
                    .HasMany(e => e.Groups)
                    .WithOne(gm => gm.Member)
                    .HasForeignKey(e => e.OrganizationUserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    ;

                entity.HasMany(d => d.Roles)
                    .WithOne(p => p.OrganizationUser)
                    .HasForeignKey(d => d.OrganizationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                
                entity
                    .HasOne(e => e.UserInvitation)
                    .WithOne(p => p.OrganizationUser)
                    .HasForeignKey<UserInvitationEntity>(p => p.OrganizationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AssociatedOrganizationUserEntity>(entity =>
            {
                entity.ToTable("AssociatedOrganizationUser");
                               
                entity.Property(e => e.OrganizationUserId1)
                    .IsRequired();
                entity.Property(e => e.OrganizationUserId2)
                    .IsRequired();
                entity.Property(e => e.AssociationType)
                    .IsRequired();
                entity.HasQueryFilter(p => p.DeletedAt == null);
            });
            
            modelBuilder.Entity<GroupEntity>(entity =>
            {
                entity.ToTable("Group");
                               
                entity.Property(e => e.Name)
                    .IsRequired();
                
                entity
                    .HasMany(e => e.GroupMembers)
                    .WithOne(gm => gm.Group)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Restrict)
                    ;

                entity
                    .HasMany(e => e.SubGroups)
                    .WithOne(sg => sg.ParentGroup)
                    .HasForeignKey(e => e.ParentGroupId)                    
                    .OnDelete(DeleteBehavior.Restrict)
                    ;
            }); 
            
            modelBuilder.Entity<GroupMemberEntity>(entity =>
            {
                entity.ToTable("GroupMember");
                               
                entity.Property(e => e.OrganizationUserId)
                    .IsRequired();
                entity.Property(e => e.GroupId)
                    .IsRequired();
                entity.HasQueryFilter(p => p.DeletedAt == null);
            });

            modelBuilder.Entity<OrganizationUserPermissionEntity>(entity =>
            {
                entity.ToTable("OrganizationUserPermission");
                entity.HasKey(e => e.OrganizationUserPermissionEntityId);
                entity.Property(e => e.OrganizationUserId)
                    .IsRequired();
                entity.Property(e => e.UserId)
                    .IsRequired();
                entity.HasQueryFilter(p => p.DeletedAt == null);
            });


            modelBuilder.Entity<RolePermissionEntity>(entity =>
            {
                entity.ToTable("RolePermission");
                entity.HasKey(e => e.RolePermissionEntityId);
                entity.Property(e => e.OrganizationId)
                    .IsRequired();
                entity.Property(e => e.RoleId)
                    .IsRequired();
                entity.HasQueryFilter(p => p.DeletedAt == null);
            });


            modelBuilder.Entity<OrganizationUserRoleEntity>(entity =>
            {
                entity.ToTable("OrganizationUserRole");
                entity.HasKey(e => e.OrganizationUserRoleEntityId);
                entity.Property(e => e.OrganizationUserId)
                    .IsRequired();
                entity.Property(e => e.OrganizationId)
                    .IsRequired();
                entity.HasQueryFilter(p => p.DeletedAt == null);
            });

            modelBuilder.Entity<UserInvitationEntity>(entity =>
            {
                entity.ToTable("UserInvitation");
                entity.HasKey(e => e.UserInvitationEntityId);
                entity.HasIndex(e => e.InvitationGuid);
                
                entity.Property(e => e.OrganizationUserId)
                    .IsRequired();
                entity.Property(e => e.OrganizationId)
                    .IsRequired();
                entity.Property(e => e.InvitationGuid)
                    .IsRequired();
            });
            
        GenerateBaselineData(modelBuilder);
        }

        

        private static void GenerateBaselineData(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ProfileParameterEntity>()
                .HasData(ProfileParameterSeedData.Values);
        }
    }
}