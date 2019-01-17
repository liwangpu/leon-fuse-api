﻿// <auto-generated />
using System;
using Apps.OMS.Service.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Apps.OMS.Service.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190117082252_OrderDetailAddOrganId")]
    partial class OrderDetailAddOrganId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Apps.OMS.Data.Entities.Customer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Cellphone");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Telephone");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.Member", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<string>("BusinessCard");

                    b.Property<string>("City");

                    b.Property<string>("Company");

                    b.Property<string>("County");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Inviter");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Province");

                    b.Property<string>("Superior");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.MemberHierarchyParam", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<bool>("IsInner");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MemberHierarchyParams");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.MemberHierarchySetting", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MemberHierarchyParamId");

                    b.Property<string>("OrganizationId");

                    b.Property<decimal>("Rate");

                    b.HasKey("Id");

                    b.ToTable("MemberHierarchySettings");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.MemberRegistry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("Approver");

                    b.Property<string>("BusinessCard");

                    b.Property<string>("City");

                    b.Property<string>("Company");

                    b.Property<string>("County");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Inviter");

                    b.Property<bool>("IsApprove");

                    b.Property<string>("Mail");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("Phone");

                    b.Property<string>("Province");

                    b.HasKey("Id");

                    b.ToTable("MemberRegistries");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.NationalUrban", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CodeNumber");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("NationalUrbanType");

                    b.Property<string>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("NationalUrbans");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("CustomerId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("SubOrderIds");

                    b.Property<string>("WorkFlowItemId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.OrderDetail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AttachmentIds");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedTime");

                    b.Property<string>("Modifier");

                    b.Property<string>("Name");

                    b.Property<int>("Num");

                    b.Property<string>("OrderId");

                    b.Property<string>("OrganizationId");

                    b.Property<string>("ProductSpecId");

                    b.Property<string>("Remark");

                    b.Property<decimal>("TotalPrice");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.OrderFlowLog", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Approve");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("OrderId");

                    b.Property<string>("Remark");

                    b.Property<string>("WorkFlowItemId");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderFlowLogs");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.OrderPointExchange", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("OrganizationId");

                    b.Property<decimal>("Rate");

                    b.HasKey("Id");

                    b.ToTable("OrderPointExchanges");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.Order", b =>
                {
                    b.HasOne("Apps.OMS.Data.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.OrderDetail", b =>
                {
                    b.HasOne("Apps.OMS.Data.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("Apps.OMS.Data.Entities.OrderFlowLog", b =>
                {
                    b.HasOne("Apps.OMS.Data.Entities.Order", "Order")
                        .WithMany("OrderFlowLogs")
                        .HasForeignKey("OrderId");
                });
#pragma warning restore 612, 618
        }
    }
}
