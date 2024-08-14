﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ForecastsDbContext))]
    [Migration("20240814094536_InitialCreateForecasts")]
    partial class InitialCreateForecasts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("game_cards")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities.PlayerCardForecast", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<Guid>("CardExternalId")
                        .HasColumnType("uuid")
                        .HasColumnName("card_external_id")
                        .HasColumnOrder(2);

                    b.Property<int?>("MlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id")
                        .HasColumnOrder(3);

                    b.Property<int>("OverallRating")
                        .HasColumnType("smallint")
                        .HasColumnName("overall_rating")
                        .HasColumnOrder(5);

                    b.Property<string>("PrimaryPosition")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("position")
                        .HasColumnOrder(4);

                    b.Property<ushort>("Year")
                        .HasColumnType("smallint")
                        .HasColumnName("year")
                        .HasColumnOrder(1);

                    b.HasKey("Id")
                        .HasName("player_card_forecasts_pkey");

                    b.HasIndex(new[] { "Year", "CardExternalId" }, "player_card_forecasts_year_card_external_id_idx");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex(new[] { "Year", "CardExternalId" }, "player_card_forecasts_year_card_external_id_idx"), "btree");

                    b.HasIndex(new[] { "Year", "MlbId" }, "player_card_forecasts_year_player_mlb_id_idx");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex(new[] { "Year", "MlbId" }, "player_card_forecasts_year_player_mlb_id_idx"), "btree");

                    b.ToTable("player_card_forecasts", "game_cards");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact", b =>
                {
                    b.Property<Guid>("player_card_forecast_id")
                        .HasColumnType("uuid");

                    b.Property<string>("type")
                        .HasColumnType("text");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date")
                        .HasColumnOrder(0);

                    b.HasKey("player_card_forecast_id", "type", "EndDate")
                        .HasName("player_card_forecast_impacts_pkey");

                    b.ToTable("player_card_forecast_impacts", "game_cards");

                    b.HasDiscriminator<string>("type").HasValue("ForecastImpact");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts.PlayerActivationForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.HasDiscriminator().HasValue("PlayerActivation");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts.PlayerDeactivationForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.HasDiscriminator().HasValue("PlayerDeactivation");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts.PlayerFreeAgencyForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.HasDiscriminator().HasValue("PlayerFreeAgency");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts.PlayerTeamSigningForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.HasDiscriminator().HasValue("PlayerTeamSigning");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.BoostForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.Property<string>("BoostReason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("boost_reason");

                    b.HasDiscriminator().HasValue("Boost");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.OverallRatingChangeForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.Property<int>("NewRating")
                        .HasColumnType("smallint")
                        .HasColumnName("new_overall_rating");

                    b.Property<int>("OldRating")
                        .HasColumnType("smallint")
                        .HasColumnName("old_overall_rating");

                    b.HasDiscriminator().HasValue("OverallRatingChange");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.PositionChangeForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.Property<string>("NewPosition")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("new_position");

                    b.Property<string>("OldPosition")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("old_position");

                    b.HasDiscriminator().HasValue("PositionChange");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts.BattingStatsForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.Property<decimal>("NewScore")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("decimal(5,4)")
                        .HasColumnName("new_score");

                    b.Property<decimal>("OldScore")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("decimal(5,4)")
                        .HasColumnName("old_score");

                    b.HasDiscriminator().HasValue("BattingStats");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts.FieldingStatsForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.Property<decimal>("NewScore")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("decimal(5,4)")
                        .HasColumnName("new_score");

                    b.Property<decimal>("OldScore")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("decimal(5,4)")
                        .HasColumnName("old_score");

                    b.HasDiscriminator().HasValue("FieldingStats");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts.PitchingStatsForecastImpact", b =>
                {
                    b.HasBaseType("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact");

                    b.Property<decimal>("NewScore")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("decimal(5,4)")
                        .HasColumnName("new_score");

                    b.Property<decimal>("OldScore")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("decimal(5,4)")
                        .HasColumnName("old_score");

                    b.HasDiscriminator().HasValue("PitchingStats");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.ForecastImpact", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities.PlayerCardForecast", null)
                        .WithMany("ForecastImpactsChronologically")
                        .HasForeignKey("player_card_forecast_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("player_card_forecast_impacts_player_card_forecasts_id_fkey");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities.PlayerCardForecast", b =>
                {
                    b.Navigation("ForecastImpactsChronologically");
                });
#pragma warning restore 612, 618
        }
    }
}
