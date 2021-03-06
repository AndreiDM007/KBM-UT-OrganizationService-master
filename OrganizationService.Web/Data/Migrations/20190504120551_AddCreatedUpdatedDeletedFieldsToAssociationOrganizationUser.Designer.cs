﻿// <auto-generated />
using System;
using Kebormed.Core.OrganizationService.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kebormed.Core.OrganizationService.Web.Data.Migrations
{
    [DbContext(typeof(OrganizationServiceDataContext))]
    [Migration("20190504120551_AddCreatedUpdatedDeletedFieldsToAssociationOrganizationUser")]
    partial class AddCreatedUpdatedDeletedFieldsToAssociationOrganizationUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.AssociatedOrganizationUserEntity", b =>
                {
                    b.Property<int>("AssociatedOrganizationUserEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssociationType");

                    b.Property<long>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<long?>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<int>("OrganizationUserId1");

                    b.Property<int>("OrganizationUserId2");

                    b.Property<long?>("RollbackedAt");

                    b.Property<string>("TransactionId");

                    b.Property<long?>("UpdatedAt");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("AssociatedOrganizationUserEntityId");

                    b.HasIndex("OrganizationUserId1", "OrganizationUserId2", "AssociationType")
                        .IsUnique();

                    b.ToTable("AssociatedOrganizationUser");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.OrganizationEntity", b =>
                {
                    b.Property<int>("OrganizationEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<long?>("RollbackedAt");

                    b.Property<string>("TransactionId");

                    b.Property<long?>("UpdatedAt");

                    b.HasKey("OrganizationEntityId");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.OrganizationUserEntity", b =>
                {
                    b.Property<int>("OrganizationUserEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CreatedAt");

                    b.Property<long?>("DeletedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("OrganizationId");

                    b.Property<long?>("RollbackedAt");

                    b.Property<string>("TransactionId");

                    b.Property<long?>("UpdatedAt");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("UserType");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("OrganizationUserEntityId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganizationUser");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileEntity", b =>
                {
                    b.Property<int>("ProfileEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("DeletedAt");

                    b.Property<int>("OrganizationUserId");

                    b.Property<long?>("RollbackedAt");

                    b.Property<string>("TransactionId");

                    b.Property<long?>("UpdatedAt");

                    b.HasKey("ProfileEntityId");

                    b.HasIndex("OrganizationUserId")
                        .IsUnique();

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileParameterEntity", b =>
                {
                    b.Property<int>("ProfileParameterEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("ProfileParameterEntityId");

                    b.ToTable("ProfileParameter");

                    b.HasData(
                        new
                        {
                            ProfileParameterEntityId = 1,
                            Name = "First Name"
                        },
                        new
                        {
                            ProfileParameterEntityId = 2,
                            Name = "Last Name"
                        },
                        new
                        {
                            ProfileParameterEntityId = 3,
                            Name = "Birthdate"
                        },
                        new
                        {
                            ProfileParameterEntityId = 4,
                            Name = "Gender"
                        },
                        new
                        {
                            ProfileParameterEntityId = 5,
                            Name = "Height"
                        },
                        new
                        {
                            ProfileParameterEntityId = 6,
                            Name = "Weight"
                        },
                        new
                        {
                            ProfileParameterEntityId = 7,
                            Name = "BMI"
                        },
                        new
                        {
                            ProfileParameterEntityId = 8,
                            Name = "Therapy Start Date"
                        },
                        new
                        {
                            ProfileParameterEntityId = 9,
                            Name = "Date Collected"
                        },
                        new
                        {
                            ProfileParameterEntityId = 10,
                            Name = "Address 1"
                        },
                        new
                        {
                            ProfileParameterEntityId = 11,
                            Name = "Address 2"
                        },
                        new
                        {
                            ProfileParameterEntityId = 12,
                            Name = "City"
                        },
                        new
                        {
                            ProfileParameterEntityId = 13,
                            Name = "State"
                        },
                        new
                        {
                            ProfileParameterEntityId = 14,
                            Name = "Country"
                        },
                        new
                        {
                            ProfileParameterEntityId = 15,
                            Name = "Postal Code"
                        },
                        new
                        {
                            ProfileParameterEntityId = 16,
                            Name = "Email"
                        },
                        new
                        {
                            ProfileParameterEntityId = 17,
                            Name = "Phone 1"
                        },
                        new
                        {
                            ProfileParameterEntityId = 18,
                            Name = "Phone 2"
                        },
                        new
                        {
                            ProfileParameterEntityId = 19,
                            Name = "Phone 3"
                        },
                        new
                        {
                            ProfileParameterEntityId = 20,
                            Name = "Phone 4"
                        },
                        new
                        {
                            ProfileParameterEntityId = 21,
                            Name = "Phone 4"
                        },
                        new
                        {
                            ProfileParameterEntityId = 22,
                            Name = "Generic 1"
                        },
                        new
                        {
                            ProfileParameterEntityId = 23,
                            Name = "Generic 2"
                        },
                        new
                        {
                            ProfileParameterEntityId = 24,
                            Name = "Generic 3"
                        },
                        new
                        {
                            ProfileParameterEntityId = 25,
                            Name = "Generic 4"
                        },
                        new
                        {
                            ProfileParameterEntityId = 26,
                            Name = "Generic 5"
                        },
                        new
                        {
                            ProfileParameterEntityId = 27,
                            Name = "Physician"
                        },
                        new
                        {
                            ProfileParameterEntityId = 28,
                            Name = "Primary Care Physician"
                        },
                        new
                        {
                            ProfileParameterEntityId = 29,
                            Name = "Group"
                        },
                        new
                        {
                            ProfileParameterEntityId = 30,
                            Name = "Sleep Doctor"
                        },
                        new
                        {
                            ProfileParameterEntityId = 31,
                            Name = "Practice"
                        },
                        new
                        {
                            ProfileParameterEntityId = 32,
                            Name = "PatientId"
                        },
                        new
                        {
                            ProfileParameterEntityId = 33,
                            Name = "PatientId2"
                        },
                        new
                        {
                            ProfileParameterEntityId = 34,
                            Name = "DeviceEditPermission"
                        });
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileValueEntity", b =>
                {
                    b.Property<int>("ProfileValueEntityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProfileId");

                    b.Property<int>("ProfileParameterId");

                    b.Property<long?>("RollbackedAt");

                    b.Property<string>("TransactionId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("ProfileValueEntityId");

                    b.HasIndex("ProfileId");

                    b.HasIndex("ProfileParameterId");

                    b.ToTable("ProfileValue");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.AssociatedOrganizationUserEntity", b =>
                {
                    b.HasOne("Kebormed.Core.OrganizationService.Web.Data.Entities.OrganizationUserEntity", "OrganizationUser")
                        .WithMany("AssociatedOrganizationUsers")
                        .HasForeignKey("OrganizationUserId1")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.OrganizationUserEntity", b =>
                {
                    b.HasOne("Kebormed.Core.OrganizationService.Web.Data.Entities.OrganizationEntity", "Organization")
                        .WithMany("OrganizationUsers")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileEntity", b =>
                {
                    b.HasOne("Kebormed.Core.OrganizationService.Web.Data.Entities.OrganizationUserEntity", "OrganizationUser")
                        .WithOne("Profile")
                        .HasForeignKey("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileEntity", "OrganizationUserId");
                });

            modelBuilder.Entity("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileValueEntity", b =>
                {
                    b.HasOne("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileEntity", "Profile")
                        .WithMany("ProfileValues")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Kebormed.Core.OrganizationService.Web.Data.Entities.ProfileParameterEntity", "ProfileParameter")
                        .WithMany("ProfileValues")
                        .HasForeignKey("ProfileParameterId");
                });
#pragma warning restore 612, 618
        }
    }
}
