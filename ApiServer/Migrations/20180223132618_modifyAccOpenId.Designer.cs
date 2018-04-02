﻿// <auto-generated />
using ApiServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ApiServer.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20180223132618_modifyAccOpenId")]
    partial class modifyAccOpenId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("ApiModel.Account", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("Bio");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("DepartmentId");

                    b.Property<DateTime>("ExpireTime");

                    b.Property<bool>("Frozened");

                    b.Property<DateTime>("LastLoginTime");

                    b.Property<string>("Mail");

                    b.Property<bool>("MailValid");

                    b.Property<string>("Nickname");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<bool>("PhoneValid");

                    b.Property<DateTime>("UnlockTime");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ApiModel.AccountOpenId", b =>
                {
                    b.Property<string>("OpenId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Platform");

                    b.HasKey("OpenId");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountOpenId");
                });

            modelBuilder.Entity("ApiModel.AssetCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("ParentId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("AssetCategories");
                });

            modelBuilder.Entity("ApiModel.AssetFolder", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("AssetFolders");
                });

            modelBuilder.Entity("ApiModel.AssetTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("AssetTags");
                });

            modelBuilder.Entity("ApiModel.ClientAsset", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("CategoryId");

                    b.Property<string>("ClassName");

                    b.Property<string>("ClientFiles");

                    b.Property<string>("Description");

                    b.Property<string>("FolderId");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Properties");

                    b.Property<string>("SrcFileLocalPath");

                    b.Property<string>("SrcFileMd5");

                    b.Property<string>("SrcFileUrl");

                    b.Property<string>("UploadTime");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("ClientAssets");
                });

            modelBuilder.Entity("ApiModel.Department", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avartar");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("ParentId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("ApiModel.Layout", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("Address");

                    b.Property<string>("CategoryId");

                    b.Property<string>("City");

                    b.Property<string>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("District");

                    b.Property<string>("FolderId");

                    b.Property<string>("GeoPos");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("PlanImageScale");

                    b.Property<string>("PlanImageUrl");

                    b.Property<string>("Province");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Layouts");
                });

            modelBuilder.Entity("ApiModel.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("ChildOrders");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("State");

                    b.Property<DateTime>("StateTime");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ApiModel.OrderStateItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Detail");

                    b.Property<string>("NewState");

                    b.Property<string>("OldState");

                    b.Property<DateTime>("OperateTime");

                    b.Property<string>("OperatorAccount");

                    b.Property<string>("OrderId");

                    b.Property<string>("Reason");

                    b.Property<string>("SolutionId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("SolutionId");

                    b.ToTable("OrderStateItem");
                });

            modelBuilder.Entity("ApiModel.Organization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avartar");

                    b.Property<string>("Bio");

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerId");

                    b.Property<string>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("ApiModel.OrganMember", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("DepartmentId");

                    b.Property<DateTime>("JoinDepartmentTime");

                    b.Property<DateTime>("JoinOrganTime");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganMember");
                });

            modelBuilder.Entity("ApiModel.Product", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("FolderId");

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ApiModel.ProductSpec", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<string>("Materials");

                    b.Property<string>("Mesh");

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.Property<string>("ProductId");

                    b.Property<string>("TPID");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSpec");
                });

            modelBuilder.Entity("ApiModel.SettingsItem", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Key");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("ApiModel.Skirting", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("FolderId");

                    b.Property<int>("Height");

                    b.Property<string>("Icon");

                    b.Property<string>("LateralPath");

                    b.Property<string>("Name");

                    b.Property<int>("Thickness");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Skirtings");
                });

            modelBuilder.Entity("ApiModel.Solution", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("CategoryId");

                    b.Property<string>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("FolderId");

                    b.Property<string>("Icon");

                    b.Property<string>("LayoutId");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LayoutId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("ApiModel.Account", b =>
                {
                    b.HasOne("ApiModel.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("ApiModel.AccountOpenId", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany("OpenIds")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("ApiModel.AssetFolder", b =>
                {
                    b.HasOne("ApiModel.Account")
                        .WithMany("Folders")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Organization")
                        .WithMany("Folders")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.ClientAsset", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany("ClientAssets")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Organization")
                        .WithMany("ClientAssets")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Department", b =>
                {
                    b.HasOne("ApiModel.Organization", "Organization")
                        .WithMany("Departments")
                        .HasForeignKey("OrganizationId");

                    b.HasOne("ApiModel.Department", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("ApiModel.Layout", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany("Layouts")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Organization")
                        .WithMany("Layouts")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Order", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany("Orders")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Organization")
                        .WithMany("Orders")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.OrderStateItem", b =>
                {
                    b.HasOne("ApiModel.Order", "Order")
                        .WithMany("OrderStates")
                        .HasForeignKey("OrderId");

                    b.HasOne("ApiModel.Solution", "Solution")
                        .WithMany()
                        .HasForeignKey("SolutionId");
                });

            modelBuilder.Entity("ApiModel.Organization", b =>
                {
                    b.HasOne("ApiModel.Account", "Owner")
                        .WithOne("Organization")
                        .HasForeignKey("ApiModel.Organization", "OwnerId");

                    b.HasOne("ApiModel.Organization", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("ApiModel.OrganMember", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Department", "Department")
                        .WithMany("Members")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("ApiModel.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Product", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany("Products")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Organization")
                        .WithMany("Products")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.ProductSpec", b =>
                {
                    b.HasOne("ApiModel.Product", "Product")
                        .WithMany("Specifications")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("ApiModel.Skirting", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("ApiModel.Solution", b =>
                {
                    b.HasOne("ApiModel.Account", "Account")
                        .WithMany("Solutions")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Layout", "Layout")
                        .WithMany()
                        .HasForeignKey("LayoutId");

                    b.HasOne("ApiModel.Organization")
                        .WithMany("Solutions")
                        .HasForeignKey("OrganizationId");
                });
#pragma warning restore 612, 618
        }
    }
}
