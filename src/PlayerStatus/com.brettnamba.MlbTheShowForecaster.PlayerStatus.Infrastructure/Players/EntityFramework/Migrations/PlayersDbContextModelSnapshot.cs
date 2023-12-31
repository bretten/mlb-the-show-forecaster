﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework.Migrations
{
    [DbContext(typeof(PlayersDbContext))]
    partial class PlayersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("players")
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean")
                        .HasColumnName("active");

                    b.Property<string>("BatSide")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("bat_side");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("date")
                        .HasColumnName("birthdate");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<DateTime>("MlbDebutDate")
                        .HasColumnType("date")
                        .HasColumnName("mlb_debut_date");

                    b.Property<int>("MlbId")
                        .HasColumnType("integer")
                        .HasColumnName("mlb_id");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("position");

                    b.Property<string>("Team")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("team");

                    b.Property<string>("ThrowArm")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("throw_arm");

                    b.HasKey("Id");

                    b.ToTable("players", "players");
                });
#pragma warning restore 612, 618
        }
    }
}
