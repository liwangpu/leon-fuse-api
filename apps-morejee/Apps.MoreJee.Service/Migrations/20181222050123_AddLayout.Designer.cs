﻿// <auto-generated />
using System;
using Apps.MoreJee.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Apps.MoreJee.Service.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20181222050123_AddLayout")]
    partial class AddLayout
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.AssetCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<int>("DisplayIndex");

                    b.Property<string>("Icon");

                    b.Property<bool>("IsRoot");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("AssetCategories");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.AssetCategoryTree", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LValue");

                    b.Property<string>("NodeType");

                    b.Property<string>("ObjId");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ParentId");

                    b.Property<int>("RValue");

                    b.HasKey("Id");

                    b.ToTable("AssetCategoryTrees");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.Map", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Data");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<string>("FileAssetId");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("PackageName");

                    b.Property<string>("Properties");

                    b.Property<string>("UnCookedAssetId");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.Material", b =>
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

                    b.Property<string>("PackageName");

                    b.Property<string>("Parameters");

                    b.Property<string>("Template");

                    b.Property<string>("UnCookedAssetId");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("CategoryId");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("DefaultSpecId");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.ProductSpec", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("Components");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<decimal>("PartnerPrice");

                    b.Property<decimal>("Price");

                    b.Property<string>("ProductId");

                    b.Property<decimal>("PurchasePrice");

                    b.Property<string>("StaticMeshs");

                    b.Property<string>("TPID");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSpecs");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.StaticMesh", b =>
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

                    b.Property<string>("Materials");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("PackageName");

                    b.Property<string>("Properties");

                    b.Property<string>("SrcFileAssetId");

                    b.Property<string>("UnCookedAssetId");

                    b.HasKey("Id");

                    b.ToTable("StaticMeshs");
                });

            modelBuilder.Entity("Apps.MoreJee.Data.Entities.ProductSpec", b =>
                {
                    b.HasOne("Apps.MoreJee.Data.Entities.Product", "Product")
                        .WithMany("Specifications")
                        .HasForeignKey("ProductId");
                });
#pragma warning restore 612, 618
        }
    }
}
