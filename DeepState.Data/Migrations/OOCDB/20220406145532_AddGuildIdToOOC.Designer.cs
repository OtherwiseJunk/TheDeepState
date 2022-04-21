﻿// <auto-generated />
using System;
using DeepState.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeepState.Data.Migrations.OOCDB
{
    [DbContext(typeof(OOCDBContext))]
    [Migration("20220406145532_AddGuildIdToOOC")]
    partial class AddGuildIdToOOC
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeepState.Data.Models.OOCItem", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateStored")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DiscordGuildId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ReportingUserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("ItemID");

                    b.ToTable("OutOfContextRecords");
                });
#pragma warning restore 612, 618
        }
    }
}