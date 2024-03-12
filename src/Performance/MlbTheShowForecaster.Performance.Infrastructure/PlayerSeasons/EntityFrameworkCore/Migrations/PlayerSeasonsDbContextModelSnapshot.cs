﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(PlayerSeasonsDbContext))]
    partial class PlayerSeasonsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("performance")
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id");

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("integer")
                        .HasColumnName("season");

                    b.HasKey("Id");

                    b.ToTable("player_stats_by_seasons", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerBattingStatsByGame", b =>
                {
                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id");

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("integer")
                        .HasColumnName("season");

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("GameMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("game_mlb_id");

                    b.Property<int>("AirOuts")
                        .HasColumnType("integer")
                        .HasColumnName("air_outs");

                    b.Property<int>("AtBats")
                        .HasColumnType("integer")
                        .HasColumnName("at_bats");

                    b.Property<int>("BaseOnBalls")
                        .HasColumnType("integer")
                        .HasColumnName("base_on_balls");

                    b.Property<int>("CatcherInterferences")
                        .HasColumnType("integer")
                        .HasColumnName("catcher_interferences");

                    b.Property<int>("CaughtStealing")
                        .HasColumnType("integer")
                        .HasColumnName("caught_stealing");

                    b.Property<int>("Doubles")
                        .HasColumnType("integer")
                        .HasColumnName("doubles");

                    b.Property<int>("GroundIntoDoublePlays")
                        .HasColumnType("integer")
                        .HasColumnName("ground_into_double_plays");

                    b.Property<int>("GroundIntoTriplePlays")
                        .HasColumnType("integer")
                        .HasColumnName("ground_into_triple_plays");

                    b.Property<int>("GroundOuts")
                        .HasColumnType("integer")
                        .HasColumnName("ground_outs");

                    b.Property<int>("HitByPitches")
                        .HasColumnType("integer")
                        .HasColumnName("hit_by_pitches");

                    b.Property<int>("Hits")
                        .HasColumnType("integer")
                        .HasColumnName("hits");

                    b.Property<int>("HomeRuns")
                        .HasColumnType("integer")
                        .HasColumnName("home_runs");

                    b.Property<int>("IntentionalWalks")
                        .HasColumnType("integer")
                        .HasColumnName("intentional_walks");

                    b.Property<int>("LeftOnBase")
                        .HasColumnType("integer")
                        .HasColumnName("left_on_base");

                    b.Property<int>("NumberOfPitchesSeen")
                        .HasColumnType("integer")
                        .HasColumnName("number_of_pitches_seen");

                    b.Property<int>("PlateAppearances")
                        .HasColumnType("integer")
                        .HasColumnName("plate_appearances");

                    b.Property<int>("Runs")
                        .HasColumnType("integer")
                        .HasColumnName("runs");

                    b.Property<int>("RunsBattedIn")
                        .HasColumnType("integer")
                        .HasColumnName("runs_batted_in");

                    b.Property<int>("SacrificeBunts")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_bunts");

                    b.Property<int>("SacrificeFlies")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_flies");

                    b.Property<int>("StolenBases")
                        .HasColumnType("integer")
                        .HasColumnName("stolen_bases");

                    b.Property<int>("Strikeouts")
                        .HasColumnType("integer")
                        .HasColumnName("strikeouts");

                    b.Property<int>("TeamMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("team_mlb_id");

                    b.Property<int>("Triples")
                        .HasColumnType("integer")
                        .HasColumnName("triples");

                    b.Property<Guid>("player_stats_by_season_id")
                        .HasColumnType("uuid");

                    b.HasKey("PlayerMlbId", "SeasonYear", "GameDate", "GameMlbId");

                    b.HasIndex("player_stats_by_season_id");

                    b.ToTable("player_batting_stats_by_games", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerFieldingStatsByGame", b =>
                {
                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id");

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("integer")
                        .HasColumnName("season");

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("GameMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("game_mlb_id");

                    b.Property<int>("Assists")
                        .HasColumnType("integer")
                        .HasColumnName("assists");

                    b.Property<int>("CatcherInterferences")
                        .HasColumnType("integer")
                        .HasColumnName("catcher_interferences");

                    b.Property<int>("CaughtStealing")
                        .HasColumnType("integer")
                        .HasColumnName("caught_stealing");

                    b.Property<int>("DoublePlays")
                        .HasColumnType("integer")
                        .HasColumnName("double_plays");

                    b.Property<int>("Errors")
                        .HasColumnType("integer")
                        .HasColumnName("errors");

                    b.Property<int>("GamesStarted")
                        .HasColumnType("integer")
                        .HasColumnName("games_started");

                    b.Property<decimal>("InningsPlayed")
                        .HasColumnType("decimal(8,3)")
                        .HasColumnName("innings_played");

                    b.Property<int>("PassedBalls")
                        .HasColumnType("integer")
                        .HasColumnName("passed_balls");

                    b.Property<int>("PickOffs")
                        .HasColumnType("integer")
                        .HasColumnName("pick_offs");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("position");

                    b.Property<int>("Putouts")
                        .HasColumnType("integer")
                        .HasColumnName("putouts");

                    b.Property<int>("StolenBases")
                        .HasColumnType("integer")
                        .HasColumnName("stolen_bases");

                    b.Property<int>("TeamMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("team_mlb_id");

                    b.Property<int>("ThrowingErrors")
                        .HasColumnType("integer")
                        .HasColumnName("throwing_errors");

                    b.Property<int>("TriplePlays")
                        .HasColumnType("integer")
                        .HasColumnName("triple_plays");

                    b.Property<int>("WildPitches")
                        .HasColumnType("integer")
                        .HasColumnName("wild_pitches");

                    b.Property<Guid>("player_stats_by_season_id")
                        .HasColumnType("uuid");

                    b.HasKey("PlayerMlbId", "SeasonYear", "GameDate", "GameMlbId");

                    b.HasIndex("player_stats_by_season_id");

                    b.ToTable("player_fielding_stats_by_games", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerPitchingStatsByGame", b =>
                {
                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id");

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("integer")
                        .HasColumnName("season");

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("GameMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("game_mlb_id");

                    b.Property<int>("AirOuts")
                        .HasColumnType("integer")
                        .HasColumnName("air_outs");

                    b.Property<int>("AtBats")
                        .HasColumnType("integer")
                        .HasColumnName("at_bats");

                    b.Property<int>("Balks")
                        .HasColumnType("integer")
                        .HasColumnName("balks");

                    b.Property<int>("BaseOnBalls")
                        .HasColumnType("integer")
                        .HasColumnName("base_on_balls");

                    b.Property<int>("BattersFaced")
                        .HasColumnType("integer")
                        .HasColumnName("batters_faced");

                    b.Property<int>("BlownSaves")
                        .HasColumnType("integer")
                        .HasColumnName("blown_saves");

                    b.Property<int>("CatcherInterferences")
                        .HasColumnType("integer")
                        .HasColumnName("catcher_interferences");

                    b.Property<int>("CaughtStealing")
                        .HasColumnType("integer")
                        .HasColumnName("caught_stealing");

                    b.Property<int>("CompleteGames")
                        .HasColumnType("integer")
                        .HasColumnName("complete_games");

                    b.Property<int>("Doubles")
                        .HasColumnType("integer")
                        .HasColumnName("doubles");

                    b.Property<int>("EarnedRuns")
                        .HasColumnType("integer")
                        .HasColumnName("earned_runs");

                    b.Property<int>("GamesFinished")
                        .HasColumnType("integer")
                        .HasColumnName("games_finished");

                    b.Property<int>("GamesStarted")
                        .HasColumnType("integer")
                        .HasColumnName("games_started");

                    b.Property<int>("GroundIntoDoublePlays")
                        .HasColumnType("integer")
                        .HasColumnName("ground_into_double_plays");

                    b.Property<int>("GroundOuts")
                        .HasColumnType("integer")
                        .HasColumnName("ground_outs");

                    b.Property<int>("HitBatsmen")
                        .HasColumnType("integer")
                        .HasColumnName("hit_batsmen");

                    b.Property<int>("Hits")
                        .HasColumnType("integer")
                        .HasColumnName("hits");

                    b.Property<int>("Holds")
                        .HasColumnType("integer")
                        .HasColumnName("holds");

                    b.Property<int>("HomeRuns")
                        .HasColumnType("integer")
                        .HasColumnName("home_runs");

                    b.Property<int>("InheritedRunners")
                        .HasColumnType("integer")
                        .HasColumnName("inherited_runners");

                    b.Property<int>("InheritedRunnersScored")
                        .HasColumnType("integer")
                        .HasColumnName("inherited_runners_scored");

                    b.Property<decimal>("InningsPitched")
                        .HasColumnType("decimal(8,3)")
                        .HasColumnName("innings_pitched");

                    b.Property<int>("IntentionalWalks")
                        .HasColumnType("integer")
                        .HasColumnName("intentional_walks");

                    b.Property<int>("Losses")
                        .HasColumnType("integer")
                        .HasColumnName("losses");

                    b.Property<int>("NumberOfPitches")
                        .HasColumnType("integer")
                        .HasColumnName("number_of_pitches");

                    b.Property<int>("Outs")
                        .HasColumnType("integer")
                        .HasColumnName("outs");

                    b.Property<int>("PickOffs")
                        .HasColumnType("integer")
                        .HasColumnName("pick_offs");

                    b.Property<int>("Runs")
                        .HasColumnType("integer")
                        .HasColumnName("runs");

                    b.Property<int>("SacrificeBunts")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_bunts");

                    b.Property<int>("SacrificeFlies")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_flies");

                    b.Property<int>("SaveOpportunities")
                        .HasColumnType("integer")
                        .HasColumnName("save_opportunities");

                    b.Property<int>("Saves")
                        .HasColumnType("integer")
                        .HasColumnName("saves");

                    b.Property<int>("Shutouts")
                        .HasColumnType("integer")
                        .HasColumnName("shutouts");

                    b.Property<int>("StolenBases")
                        .HasColumnType("integer")
                        .HasColumnName("stolen_bases");

                    b.Property<int>("Strikeouts")
                        .HasColumnType("integer")
                        .HasColumnName("strikeouts");

                    b.Property<int>("Strikes")
                        .HasColumnType("integer")
                        .HasColumnName("strikes");

                    b.Property<int>("TeamMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("team_mlb_id");

                    b.Property<int>("Triples")
                        .HasColumnType("integer")
                        .HasColumnName("triples");

                    b.Property<int>("WildPitches")
                        .HasColumnType("integer")
                        .HasColumnName("wild_pitches");

                    b.Property<int>("Wins")
                        .HasColumnType("integer")
                        .HasColumnName("wins");

                    b.Property<Guid>("player_stats_by_season_id")
                        .HasColumnType("uuid");

                    b.HasKey("PlayerMlbId", "SeasonYear", "GameDate", "GameMlbId");

                    b.HasIndex("player_stats_by_season_id");

                    b.ToTable("player_pitching_stats_by_games", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerBattingStatsByGame", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", null)
                        .WithMany("_battingStatsByGames")
                        .HasForeignKey("player_stats_by_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerFieldingStatsByGame", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", null)
                        .WithMany("_fieldingStatsByGames")
                        .HasForeignKey("player_stats_by_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerPitchingStatsByGame", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", null)
                        .WithMany("_pitchingStatsByGames")
                        .HasForeignKey("player_stats_by_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", b =>
                {
                    b.Navigation("_battingStatsByGames");

                    b.Navigation("_fieldingStatsByGames");

                    b.Navigation("_pitchingStatsByGames");
                });
#pragma warning restore 612, 618
        }
    }
}
