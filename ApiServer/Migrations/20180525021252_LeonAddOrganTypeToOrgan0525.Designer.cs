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
    [Migration("20180525021252_LeonAddOrganTypeToOrgan0525")]
    partial class LeonAddOrganTypeToOrgan0525
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("ApiModel.Entities.Account", b =>
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

                    b.Property<bool>("Frozened");

                    b.Property<string>("Icon");

                    b.Property<string>("Location");

                    b.Property<string>("Mail");

                    b.Property<bool>("MailValid");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<bool>("PhoneValid");

                    b.Property<int>("ResourceType");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ApiModel.Entities.AccountOpenId", b =>
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

            modelBuilder.Entity("ApiModel.Entities.AssetCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<int>("DisplayIndex");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<int>("ResourceType");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("AssetCategories");
                });

            modelBuilder.Entity("ApiModel.Entities.AssetCategoryTree", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LValue");

                    b.Property<string>("Name");

                    b.Property<string>("NodeType");

                    b.Property<string>("ObjId");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<int>("RValue");

                    b.HasKey("Id");

                    b.ToTable("AssetCategoryTrees");
                });

            modelBuilder.Entity("ApiModel.Entities.AssetFolder", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("AssetFolders");
                });

            modelBuilder.Entity("ApiModel.Entities.AssetTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("AssetTags");
                });

            modelBuilder.Entity("ApiModel.Entities.ClientAsset", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("ClientAssets");
                });

            modelBuilder.Entity("ApiModel.Entities.Department", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("ParentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ApiModel.Entities.FileAsset", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("FileExt");

                    b.Property<string>("Icon");

                    b.Property<string>("LocalPath");

                    b.Property<string>("Md5");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.Property<long>("Size");

                    b.Property<string>("UploadTime");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("ApiModel.Entities.Layout", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Layouts");
                });

            modelBuilder.Entity("ApiModel.Entities.Map", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<string>("FileAssetId");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Properties");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("ApiModel.Entities.Material", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<string>("FileAssetId");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Parameters");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("ApiModel.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("ChildOrders");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.Property<string>("State");

                    b.Property<DateTime>("StateTime");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ApiModel.Entities.OrderStateItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Detail");

                    b.Property<string>("Name");

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

            modelBuilder.Entity("ApiModel.Entities.Organization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<string>("Location");

                    b.Property<string>("Mail");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("OwnerId");

                    b.Property<string>("ParentId");

                    b.Property<int>("ResourceType");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("ApiModel.Entities.OrganMember", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("DepartmentId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("JoinDepartmentTime");

                    b.Property<DateTime>("JoinOrganTime");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("OrganMember");
                });

            modelBuilder.Entity("ApiModel.Entities.Package", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("ApiModel.Entities.PermissionTree", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LValue");

                    b.Property<string>("Name");

                    b.Property<string>("NodeType");

                    b.Property<string>("ObjId");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<int>("RValue");

                    b.HasKey("Id");

                    b.ToTable("PermissionTrees");
                });

            modelBuilder.Entity("ApiModel.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ApiModel.Entities.ProductSpec", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("Album");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<decimal>("Price");

                    b.Property<string>("ProductId");

                    b.Property<int>("ResourceType");

                    b.Property<string>("StaticMeshIds");

                    b.Property<string>("TPID");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSpec");
                });

            modelBuilder.Entity("ApiModel.Entities.SettingsItem", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Key");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("ApiModel.Entities.Skirting", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("Skirtings");
                });

            modelBuilder.Entity("ApiModel.Entities.Solution", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Data");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<string>("LayoutId");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LayoutId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("ApiModel.Entities.StaticMesh", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<string>("FileAssetId");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Properties");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("StaticMeshs");
                });

            modelBuilder.Entity("ApiModel.Entities.Texture", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<string>("FileAssetId");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Properties");

                    b.Property<int>("ResourceType");

                    b.HasKey("Id");

                    b.ToTable("Textures");
                });

            modelBuilder.Entity("BambooCommon.PermissionItem", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<byte>("Permission");

                    b.Property<string>("ResId");

                    b.Property<string>("ResType");

                    b.HasKey("Id");

                    b.HasIndex("AccountId", "ResId", "ResType");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("ApiModel.Entities.Account", b =>
                {
                    b.HasOne("ApiModel.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("ApiModel.Entities.AccountOpenId", b =>
                {
                    b.HasOne("ApiModel.Entities.Account", "Account")
                        .WithMany("OpenIds")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("ApiModel.Entities.AssetFolder", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Folders")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("Folders")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.ClientAsset", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("ClientAssets")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("ClientAssets")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.Department", b =>
                {
                    b.HasOne("ApiModel.Entities.Organization", "Organization")
                        .WithMany("Departments")
                        .HasForeignKey("OrganizationId");

                    b.HasOne("ApiModel.Entities.Department", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("ApiModel.Entities.FileAsset", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Files")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("Files")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.Layout", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Layouts")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("Layouts")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.Order", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Orders")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("Orders")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.OrderStateItem", b =>
                {
                    b.HasOne("ApiModel.Entities.Order", "Order")
                        .WithMany("OrderStates")
                        .HasForeignKey("OrderId");

                    b.HasOne("ApiModel.Entities.Solution", "Solution")
                        .WithMany()
                        .HasForeignKey("SolutionId");
                });

            modelBuilder.Entity("ApiModel.Entities.Organization", b =>
                {
                    b.HasOne("ApiModel.Entities.Account", "Owner")
                        .WithOne("Organization")
                        .HasForeignKey("ApiModel.Entities.Organization", "OwnerId");

                    b.HasOne("ApiModel.Entities.Organization", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("ApiModel.Entities.OrganMember", b =>
                {
                    b.HasOne("ApiModel.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Department", "Department")
                        .WithMany("Members")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("ApiModel.Entities.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.Product", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Products")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("Products")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ApiModel.Entities.ProductSpec", b =>
                {
                    b.HasOne("ApiModel.Entities.Product", "Product")
                        .WithMany("Specifications")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("ApiModel.Entities.Solution", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Solutions")
                        .HasForeignKey("AccountId");

                    b.HasOne("ApiModel.Entities.Layout", "Layout")
                        .WithMany()
                        .HasForeignKey("LayoutId");

                    b.HasOne("ApiModel.Entities.Organization")
                        .WithMany("Solutions")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("BambooCommon.PermissionItem", b =>
                {
                    b.HasOne("ApiModel.Entities.Account")
                        .WithMany("Permissions")
                        .HasForeignKey("AccountId");
                });
#pragma warning restore 612, 618
        }
    }
}
