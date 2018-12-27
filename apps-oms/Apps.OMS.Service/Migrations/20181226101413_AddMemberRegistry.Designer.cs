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
    [Migration("20181226101413_AddMemberRegistry")]
    partial class AddMemberRegistry
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Apps.OMS.Data.Entities.MemberRegistry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActiveFlag");

                    b.Property<string>("Approver");

                    b.Property<string>("Area");

                    b.Property<string>("BusinessCard");

                    b.Property<string>("City");

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("Creator");

                    b.Property<string>("Description");

                    b.Property<string>("Icon");

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
#pragma warning restore 612, 618
        }
    }
}