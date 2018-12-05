﻿// <auto-generated />
using System;
using Apps.Basic.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Apps.Basic.Service.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Apps.Basic.Data.Entities.Account", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActivationTime");

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("DepartmentId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ExpireTime");

                    b.Property<string>("Icon");

                    b.Property<string>("InnerRoleId");

                    b.Property<string>("Location");

                    b.Property<string>("Mail");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("InnerRoleId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.Department", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.FileAsset", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("FileExt");

                    b.Property<string>("Icon");

                    b.Property<string>("LocalPath");

                    b.Property<string>("Md5");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<long>("Size");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.Navigation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Field");

                    b.Property<string>("Icon");

                    b.Property<bool>("IsInner");

                    b.Property<string>("Name");

                    b.Property<bool>("NewTapOpen");

                    b.Property<string>("NodeType");

                    b.Property<string>("PagedModel");

                    b.Property<string>("Permission");

                    b.Property<string>("QueryParams");

                    b.Property<string>("Resource");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Navigations");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.Organization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActivationTime");

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ExpireTime");

                    b.Property<string>("Location");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationTypeId");

                    b.Property<string>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationTypeId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.OrganizationTree", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LValue");

                    b.Property<string>("NodeType");

                    b.Property<string>("ObjId");

                    b.Property<string>("ParentId");

                    b.Property<int>("RValue");

                    b.HasKey("Id");

                    b.ToTable("OrganizationTrees");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.OrganizationType", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<bool>("IsInner");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("OrganizationTypes");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.UserNav", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("RoleId");

                    b.HasKey("Id");

                    b.ToTable("UserNavs");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.UserNavDetail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ExcludeFiled");

                    b.Property<string>("ExcludePermission");

                    b.Property<string>("ExcludeQueryParams");

                    b.Property<int>("Grade");

                    b.Property<string>("ParentId");

                    b.Property<string>("RefNavigationId");

                    b.Property<string>("UserNavId");

                    b.HasKey("Id");

                    b.HasIndex("UserNavId");

                    b.ToTable("UserNavDetails");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<bool>("IsInner");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.Account", b =>
                {
                    b.HasOne("Apps.Basic.Data.Entities.Department", "Department")
                        .WithMany("Accounts")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("Apps.Basic.Data.Entities.UserRole", "InnerRole")
                        .WithMany()
                        .HasForeignKey("InnerRoleId");

                    b.HasOne("Apps.Basic.Data.Entities.Organization", "Organization")
                        .WithMany("Accounts")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.Organization", b =>
                {
                    b.HasOne("Apps.Basic.Data.Entities.OrganizationType", "OrganizationType")
                        .WithMany("Organizations")
                        .HasForeignKey("OrganizationTypeId");
                });

            modelBuilder.Entity("Apps.Basic.Data.Entities.UserNavDetail", b =>
                {
                    b.HasOne("Apps.Basic.Data.Entities.UserNav", "UserNav")
                        .WithMany("UserNavDetails")
                        .HasForeignKey("UserNavId");
                });
#pragma warning restore 612, 618
        }
    }
}
