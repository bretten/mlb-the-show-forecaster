﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(PlayerSeasonsDbContext))]
    [ExcludeFromCodeCoverage]
    partial class PlayerSeasonsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("performance")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id")
                        .HasColumnOrder(1);

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("smallint")
                        .HasColumnName("season")
                        .HasColumnOrder(2);

                    b.HasKey("Id")
                        .HasName("player_stats_by_seasons_pkey");

                    b.HasIndex(new[] { "SeasonYear" }, "player_stats_by_seasons_season_idx");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex(new[] { "SeasonYear" }, "player_stats_by_seasons_season_idx"), "btree");

                    b.ToTable("player_stats_by_seasons", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerBattingStatsByGame", b =>
                {
                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id")
                        .HasColumnOrder(1);

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("smallint")
                        .HasColumnName("season")
                        .HasColumnOrder(2);

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("date")
                        .HasColumnName("date")
                        .HasColumnOrder(3);

                    b.Property<int>("GameMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("game_mlb_id")
                        .HasColumnOrder(4);

                    b.Property<int>("AirOuts")
                        .HasColumnType("integer")
                        .HasColumnName("air_outs")
                        .HasColumnOrder(27);

                    b.Property<int>("AtBats")
                        .HasColumnType("integer")
                        .HasColumnName("at_bats")
                        .HasColumnOrder(7);

                    b.Property<int>("BaseOnBalls")
                        .HasColumnType("integer")
                        .HasColumnName("base_on_balls")
                        .HasColumnOrder(14);

                    b.Property<int>("CatcherInterferences")
                        .HasColumnType("integer")
                        .HasColumnName("catcher_interferences")
                        .HasColumnOrder(28);

                    b.Property<int>("CaughtStealing")
                        .HasColumnType("integer")
                        .HasColumnName("caught_stealing")
                        .HasColumnOrder(18);

                    b.Property<int>("Doubles")
                        .HasColumnType("integer")
                        .HasColumnName("doubles")
                        .HasColumnOrder(10);

                    b.Property<int>("GroundIntoDoublePlays")
                        .HasColumnType("integer")
                        .HasColumnName("ground_into_double_plays")
                        .HasColumnOrder(25);

                    b.Property<int>("GroundIntoTriplePlays")
                        .HasColumnType("integer")
                        .HasColumnName("ground_into_triple_plays")
                        .HasColumnOrder(26);

                    b.Property<int>("GroundOuts")
                        .HasColumnType("integer")
                        .HasColumnName("ground_outs")
                        .HasColumnOrder(24);

                    b.Property<int>("HitByPitches")
                        .HasColumnType("integer")
                        .HasColumnName("hit_by_pitches")
                        .HasColumnOrder(19);

                    b.Property<int>("Hits")
                        .HasColumnType("integer")
                        .HasColumnName("hits")
                        .HasColumnOrder(9);

                    b.Property<int>("HomeRuns")
                        .HasColumnType("integer")
                        .HasColumnName("home_runs")
                        .HasColumnOrder(12);

                    b.Property<int>("IntentionalWalks")
                        .HasColumnType("integer")
                        .HasColumnName("intentional_walks")
                        .HasColumnOrder(15);

                    b.Property<int>("LeftOnBase")
                        .HasColumnType("integer")
                        .HasColumnName("left_on_base")
                        .HasColumnOrder(23);

                    b.Property<int>("NumberOfPitchesSeen")
                        .HasColumnType("integer")
                        .HasColumnName("number_of_pitches_seen")
                        .HasColumnOrder(22);

                    b.Property<int>("PlateAppearances")
                        .HasColumnType("integer")
                        .HasColumnName("plate_appearances")
                        .HasColumnOrder(6);

                    b.Property<int>("Runs")
                        .HasColumnType("integer")
                        .HasColumnName("runs")
                        .HasColumnOrder(8);

                    b.Property<int>("RunsBattedIn")
                        .HasColumnType("integer")
                        .HasColumnName("runs_batted_in")
                        .HasColumnOrder(13);

                    b.Property<int>("SacrificeBunts")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_bunts")
                        .HasColumnOrder(20);

                    b.Property<int>("SacrificeFlies")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_flies")
                        .HasColumnOrder(21);

                    b.Property<int>("StolenBases")
                        .HasColumnType("integer")
                        .HasColumnName("stolen_bases")
                        .HasColumnOrder(17);

                    b.Property<int>("Strikeouts")
                        .HasColumnType("integer")
                        .HasColumnName("strikeouts")
                        .HasColumnOrder(16);

                    b.Property<int>("TeamMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("team_mlb_id")
                        .HasColumnOrder(5);

                    b.Property<int>("Triples")
                        .HasColumnType("integer")
                        .HasColumnName("triples")
                        .HasColumnOrder(11);

                    b.Property<Guid>("player_stats_by_season_id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.HasKey("PlayerMlbId", "SeasonYear", "GameDate", "GameMlbId")
                        .HasName("player_batting_stats_by_games_pkey");

                    b.HasIndex("player_stats_by_season_id");

                    b.ToTable("player_batting_stats_by_games", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerFieldingStatsByGame", b =>
                {
                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id")
                        .HasColumnOrder(1);

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("smallint")
                        .HasColumnName("season")
                        .HasColumnOrder(2);

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("date")
                        .HasColumnName("date")
                        .HasColumnOrder(3);

                    b.Property<int>("GameMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("game_mlb_id")
                        .HasColumnOrder(4);

                    b.Property<int>("Assists")
                        .HasColumnType("integer")
                        .HasColumnName("assists")
                        .HasColumnOrder(9);

                    b.Property<int>("CatcherInterferences")
                        .HasColumnType("integer")
                        .HasColumnName("catcher_interferences")
                        .HasColumnOrder(18);

                    b.Property<int>("CaughtStealing")
                        .HasColumnType("integer")
                        .HasColumnName("caught_stealing")
                        .HasColumnOrder(15);

                    b.Property<int>("DoublePlays")
                        .HasColumnType("integer")
                        .HasColumnName("double_plays")
                        .HasColumnOrder(13);

                    b.Property<int>("Errors")
                        .HasColumnType("integer")
                        .HasColumnName("errors")
                        .HasColumnOrder(11);

                    b.Property<int>("GamesStarted")
                        .HasColumnType("integer")
                        .HasColumnName("games_started")
                        .HasColumnOrder(7);

                    b.Property<decimal>("InningsPlayed")
                        .HasColumnType("decimal(8,3)")
                        .HasColumnName("innings_played")
                        .HasColumnOrder(8);

                    b.Property<int>("PassedBalls")
                        .HasColumnType("integer")
                        .HasColumnName("passed_balls")
                        .HasColumnOrder(17);

                    b.Property<int>("Pickoffs")
                        .HasColumnType("integer")
                        .HasColumnName("pickoffs")
                        .HasColumnOrder(20);

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(4)")
                        .HasColumnName("position")
                        .HasColumnOrder(6);

                    b.Property<int>("Putouts")
                        .HasColumnType("integer")
                        .HasColumnName("putouts")
                        .HasColumnOrder(10);

                    b.Property<int>("StolenBases")
                        .HasColumnType("integer")
                        .HasColumnName("stolen_bases")
                        .HasColumnOrder(16);

                    b.Property<int>("TeamMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("team_mlb_id")
                        .HasColumnOrder(5);

                    b.Property<int>("ThrowingErrors")
                        .HasColumnType("integer")
                        .HasColumnName("throwing_errors")
                        .HasColumnOrder(12);

                    b.Property<int>("TriplePlays")
                        .HasColumnType("integer")
                        .HasColumnName("triple_plays")
                        .HasColumnOrder(14);

                    b.Property<int>("WildPitches")
                        .HasColumnType("integer")
                        .HasColumnName("wild_pitches")
                        .HasColumnOrder(19);

                    b.Property<Guid>("player_stats_by_season_id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.HasKey("PlayerMlbId", "SeasonYear", "GameDate", "GameMlbId")
                        .HasName("player_fielding_stats_by_games_pkey");

                    b.HasIndex("player_stats_by_season_id");

                    b.ToTable("player_fielding_stats_by_games", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerPitchingStatsByGame", b =>
                {
                    b.Property<int>("PlayerMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("player_mlb_id")
                        .HasColumnOrder(1);

                    b.Property<ushort>("SeasonYear")
                        .HasColumnType("smallint")
                        .HasColumnName("season")
                        .HasColumnOrder(2);

                    b.Property<DateTime>("GameDate")
                        .HasColumnType("date")
                        .HasColumnName("date")
                        .HasColumnOrder(3);

                    b.Property<int>("GameMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("game_mlb_id")
                        .HasColumnOrder(4);

                    b.Property<int>("AirOuts")
                        .HasColumnType("integer")
                        .HasColumnName("air_outs")
                        .HasColumnOrder(29);

                    b.Property<int>("AtBats")
                        .HasColumnType("integer")
                        .HasColumnName("at_bats")
                        .HasColumnOrder(36);

                    b.Property<int>("Balks")
                        .HasColumnType("integer")
                        .HasColumnName("balks")
                        .HasColumnOrder(34);

                    b.Property<int>("BaseOnBalls")
                        .HasColumnType("integer")
                        .HasColumnName("base_on_balls")
                        .HasColumnOrder(24);

                    b.Property<int>("BattersFaced")
                        .HasColumnType("integer")
                        .HasColumnName("batters_faced")
                        .HasColumnOrder(35);

                    b.Property<int>("BlownSaves")
                        .HasColumnType("integer")
                        .HasColumnName("blown_saves")
                        .HasColumnOrder(14);

                    b.Property<int>("CatcherInterferences")
                        .HasColumnType("integer")
                        .HasColumnName("catcher_interferences")
                        .HasColumnOrder(42);

                    b.Property<int>("CaughtStealing")
                        .HasColumnType("integer")
                        .HasColumnName("caught_stealing")
                        .HasColumnOrder(38);

                    b.Property<int>("CompleteGames")
                        .HasColumnType("integer")
                        .HasColumnName("complete_games")
                        .HasColumnOrder(10);

                    b.Property<int>("Doubles")
                        .HasColumnType("integer")
                        .HasColumnName("doubles")
                        .HasColumnOrder(18);

                    b.Property<int>("EarnedRuns")
                        .HasColumnType("integer")
                        .HasColumnName("earned_runs")
                        .HasColumnOrder(22);

                    b.Property<int>("GamesFinished")
                        .HasColumnType("integer")
                        .HasColumnName("games_finished")
                        .HasColumnOrder(9);

                    b.Property<int>("GamesStarted")
                        .HasColumnType("integer")
                        .HasColumnName("games_started")
                        .HasColumnOrder(8);

                    b.Property<int>("GroundIntoDoublePlays")
                        .HasColumnType("integer")
                        .HasColumnName("ground_into_double_plays")
                        .HasColumnOrder(30);

                    b.Property<int>("GroundOuts")
                        .HasColumnType("integer")
                        .HasColumnName("ground_outs")
                        .HasColumnOrder(28);

                    b.Property<int>("HitBatsmen")
                        .HasColumnType("integer")
                        .HasColumnName("hit_batsmen")
                        .HasColumnOrder(26);

                    b.Property<int>("Hits")
                        .HasColumnType("integer")
                        .HasColumnName("hits")
                        .HasColumnOrder(17);

                    b.Property<int>("Holds")
                        .HasColumnType("integer")
                        .HasColumnName("holds")
                        .HasColumnOrder(12);

                    b.Property<int>("HomeRuns")
                        .HasColumnType("integer")
                        .HasColumnName("home_runs")
                        .HasColumnOrder(20);

                    b.Property<int>("InheritedRunners")
                        .HasColumnType("integer")
                        .HasColumnName("inherited_runners")
                        .HasColumnOrder(40);

                    b.Property<int>("InheritedRunnersScored")
                        .HasColumnType("integer")
                        .HasColumnName("inherited_runners_scored")
                        .HasColumnOrder(41);

                    b.Property<decimal>("InningsPitched")
                        .HasColumnType("decimal(8,3)")
                        .HasColumnName("innings_pitched")
                        .HasColumnOrder(16);

                    b.Property<int>("IntentionalWalks")
                        .HasColumnType("integer")
                        .HasColumnName("intentional_walks")
                        .HasColumnOrder(25);

                    b.Property<int>("Losses")
                        .HasColumnType("integer")
                        .HasColumnName("losses")
                        .HasColumnOrder(7);

                    b.Property<int>("NumberOfPitches")
                        .HasColumnType("integer")
                        .HasColumnName("number_of_pitches")
                        .HasColumnOrder(31);

                    b.Property<int>("Outs")
                        .HasColumnType("integer")
                        .HasColumnName("outs")
                        .HasColumnOrder(27);

                    b.Property<int>("Pickoffs")
                        .HasColumnType("integer")
                        .HasColumnName("pickoffs")
                        .HasColumnOrder(39);

                    b.Property<int>("Runs")
                        .HasColumnType("integer")
                        .HasColumnName("runs")
                        .HasColumnOrder(21);

                    b.Property<int>("SacrificeBunts")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_bunts")
                        .HasColumnOrder(43);

                    b.Property<int>("SacrificeFlies")
                        .HasColumnType("integer")
                        .HasColumnName("sacrifice_flies")
                        .HasColumnOrder(44);

                    b.Property<int>("SaveOpportunities")
                        .HasColumnType("integer")
                        .HasColumnName("save_opportunities")
                        .HasColumnOrder(15);

                    b.Property<int>("Saves")
                        .HasColumnType("integer")
                        .HasColumnName("saves")
                        .HasColumnOrder(13);

                    b.Property<int>("Shutouts")
                        .HasColumnType("integer")
                        .HasColumnName("shutouts")
                        .HasColumnOrder(11);

                    b.Property<int>("StolenBases")
                        .HasColumnType("integer")
                        .HasColumnName("stolen_bases")
                        .HasColumnOrder(37);

                    b.Property<int>("Strikeouts")
                        .HasColumnType("integer")
                        .HasColumnName("strikeouts")
                        .HasColumnOrder(23);

                    b.Property<int>("Strikes")
                        .HasColumnType("integer")
                        .HasColumnName("strikes")
                        .HasColumnOrder(32);

                    b.Property<int>("TeamMlbId")
                        .HasColumnType("integer")
                        .HasColumnName("team_mlb_id")
                        .HasColumnOrder(5);

                    b.Property<int>("Triples")
                        .HasColumnType("integer")
                        .HasColumnName("triples")
                        .HasColumnOrder(19);

                    b.Property<int>("WildPitches")
                        .HasColumnType("integer")
                        .HasColumnName("wild_pitches")
                        .HasColumnOrder(33);

                    b.Property<int>("Wins")
                        .HasColumnType("integer")
                        .HasColumnName("wins")
                        .HasColumnOrder(6);

                    b.Property<Guid>("player_stats_by_season_id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.HasKey("PlayerMlbId", "SeasonYear", "GameDate", "GameMlbId")
                        .HasName("player_pitching_stats_by_games_pkey");

                    b.HasIndex("player_stats_by_season_id");

                    b.ToTable("player_pitching_stats_by_games", "performance");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerBattingStatsByGame", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", null)
                        .WithMany("_battingStatsByGames")
                        .HasForeignKey("player_stats_by_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("player_batting_stats_by_games_player_stats_by_seasons_id_fkey");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerFieldingStatsByGame", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", null)
                        .WithMany("_fieldingStatsByGames")
                        .HasForeignKey("player_stats_by_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("player_fielding_stats_by_games_player_stats_by_seasons_id_fkey");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.PlayerPitchingStatsByGame", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities.PlayerStatsBySeason", null)
                        .WithMany("_pitchingStatsByGames")
                        .HasForeignKey("player_stats_by_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("player_pitching_stats_by_games_player_stats_by_seasons_id_fkey");
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
