﻿// <auto-generated />
using DeepState.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeepState.Data.Migrations.RPG
{
    [DbContext(typeof(RPGContext))]
    [Migration("20220123164337_AddCharacters")]
    partial class AddCharacters
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeepState.Data.Models.Character", b =>
                {
                    b.Property<decimal>("DiscordUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Fortitude")
                        .HasColumnType("int");

                    b.Property<int>("Gold")
                        .HasColumnType("int");

                    b.Property<int>("Hitpoints")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("Mobility")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<int>("XP")
                        .HasColumnType("int");

                    b.HasKey("DiscordUserId");

                    b.ToTable("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}