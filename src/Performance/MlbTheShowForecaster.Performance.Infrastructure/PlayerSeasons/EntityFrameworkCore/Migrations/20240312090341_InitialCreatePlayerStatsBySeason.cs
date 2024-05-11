using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class InitialCreatePlayerStatsBySeason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "performance");

            migrationBuilder.CreateTable(
                name: "player_stats_by_seasons",
                schema: "performance",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    season = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_stats_by_seasons_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_batting_stats_by_games",
                schema: "performance",
                columns: table => new
                {
                    player_stats_by_season_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    season = table.Column<short>(type: "smallint", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    game_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    team_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    plate_appearances = table.Column<int>(type: "integer", nullable: false),
                    at_bats = table.Column<int>(type: "integer", nullable: false),
                    runs = table.Column<int>(type: "integer", nullable: false),
                    hits = table.Column<int>(type: "integer", nullable: false),
                    doubles = table.Column<int>(type: "integer", nullable: false),
                    triples = table.Column<int>(type: "integer", nullable: false),
                    home_runs = table.Column<int>(type: "integer", nullable: false),
                    runs_batted_in = table.Column<int>(type: "integer", nullable: false),
                    base_on_balls = table.Column<int>(type: "integer", nullable: false),
                    intentional_walks = table.Column<int>(type: "integer", nullable: false),
                    strikeouts = table.Column<int>(type: "integer", nullable: false),
                    stolen_bases = table.Column<int>(type: "integer", nullable: false),
                    caught_stealing = table.Column<int>(type: "integer", nullable: false),
                    hit_by_pitches = table.Column<int>(type: "integer", nullable: false),
                    sacrifice_bunts = table.Column<int>(type: "integer", nullable: false),
                    sacrifice_flies = table.Column<int>(type: "integer", nullable: false),
                    number_of_pitches_seen = table.Column<int>(type: "integer", nullable: false),
                    left_on_base = table.Column<int>(type: "integer", nullable: false),
                    ground_outs = table.Column<int>(type: "integer", nullable: false),
                    ground_into_double_plays = table.Column<int>(type: "integer", nullable: false),
                    ground_into_triple_plays = table.Column<int>(type: "integer", nullable: false),
                    air_outs = table.Column<int>(type: "integer", nullable: false),
                    catcher_interferences = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_batting_stats_by_games_pkey", x => new { x.player_mlb_id, x.season, x.date, x.game_mlb_id });
                    table.ForeignKey(
                        name: "player_batting_stats_by_games_player_stats_by_seasons_id_fkey",
                        column: x => x.player_stats_by_season_id,
                        principalSchema: "performance",
                        principalTable: "player_stats_by_seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_fielding_stats_by_games",
                schema: "performance",
                columns: table => new
                {
                    player_stats_by_season_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    season = table.Column<short>(type: "smallint", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    game_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    team_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    position = table.Column<string>(type: "varchar(4)", nullable: false),
                    games_started = table.Column<int>(type: "integer", nullable: false),
                    innings_played = table.Column<decimal>(type: "numeric(8,3)", nullable: false),
                    assists = table.Column<int>(type: "integer", nullable: false),
                    putouts = table.Column<int>(type: "integer", nullable: false),
                    errors = table.Column<int>(type: "integer", nullable: false),
                    throwing_errors = table.Column<int>(type: "integer", nullable: false),
                    double_plays = table.Column<int>(type: "integer", nullable: false),
                    triple_plays = table.Column<int>(type: "integer", nullable: false),
                    caught_stealing = table.Column<int>(type: "integer", nullable: false),
                    stolen_bases = table.Column<int>(type: "integer", nullable: false),
                    passed_balls = table.Column<int>(type: "integer", nullable: false),
                    catcher_interferences = table.Column<int>(type: "integer", nullable: false),
                    wild_pitches = table.Column<int>(type: "integer", nullable: false),
                    pickoffs = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_fielding_stats_by_games_pkey", x => new { x.player_mlb_id, x.season, x.date, x.game_mlb_id });
                    table.ForeignKey(
                        name: "player_fielding_stats_by_games_player_stats_by_seasons_id_fkey",
                        column: x => x.player_stats_by_season_id,
                        principalSchema: "performance",
                        principalTable: "player_stats_by_seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_pitching_stats_by_games",
                schema: "performance",
                columns: table => new
                {
                    player_stats_by_season_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    season = table.Column<short>(type: "smallint", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    game_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    team_mlb_id = table.Column<int>(type: "integer", nullable: false),
                    wins = table.Column<int>(type: "integer", nullable: false),
                    losses = table.Column<int>(type: "integer", nullable: false),
                    games_started = table.Column<int>(type: "integer", nullable: false),
                    games_finished = table.Column<int>(type: "integer", nullable: false),
                    complete_games = table.Column<int>(type: "integer", nullable: false),
                    shutouts = table.Column<int>(type: "integer", nullable: false),
                    holds = table.Column<int>(type: "integer", nullable: false),
                    saves = table.Column<int>(type: "integer", nullable: false),
                    blown_saves = table.Column<int>(type: "integer", nullable: false),
                    save_opportunities = table.Column<int>(type: "integer", nullable: false),
                    innings_pitched = table.Column<decimal>(type: "numeric(8,3)", nullable: false),
                    hits = table.Column<int>(type: "integer", nullable: false),
                    doubles = table.Column<int>(type: "integer", nullable: false),
                    triples = table.Column<int>(type: "integer", nullable: false),
                    home_runs = table.Column<int>(type: "integer", nullable: false),
                    runs = table.Column<int>(type: "integer", nullable: false),
                    earned_runs = table.Column<int>(type: "integer", nullable: false),
                    strikeouts = table.Column<int>(type: "integer", nullable: false),
                    base_on_balls = table.Column<int>(type: "integer", nullable: false),
                    intentional_walks = table.Column<int>(type: "integer", nullable: false),
                    hit_batsmen = table.Column<int>(type: "integer", nullable: false),
                    outs = table.Column<int>(type: "integer", nullable: false),
                    ground_outs = table.Column<int>(type: "integer", nullable: false),
                    air_outs = table.Column<int>(type: "integer", nullable: false),
                    ground_into_double_plays = table.Column<int>(type: "integer", nullable: false),
                    number_of_pitches = table.Column<int>(type: "integer", nullable: false),
                    strikes = table.Column<int>(type: "integer", nullable: false),
                    wild_pitches = table.Column<int>(type: "integer", nullable: false),
                    balks = table.Column<int>(type: "integer", nullable: false),
                    batters_faced = table.Column<int>(type: "integer", nullable: false),
                    at_bats = table.Column<int>(type: "integer", nullable: false),
                    stolen_bases = table.Column<int>(type: "integer", nullable: false),
                    caught_stealing = table.Column<int>(type: "integer", nullable: false),
                    pickoffs = table.Column<int>(type: "integer", nullable: false),
                    inherited_runners = table.Column<int>(type: "integer", nullable: false),
                    inherited_runners_scored = table.Column<int>(type: "integer", nullable: false),
                    catcher_interferences = table.Column<int>(type: "integer", nullable: false),
                    sacrifice_bunts = table.Column<int>(type: "integer", nullable: false),
                    sacrifice_flies = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_pitching_stats_by_games_pkey", x => new { x.player_mlb_id, x.season, x.date, x.game_mlb_id });
                    table.ForeignKey(
                        name: "player_pitching_stats_by_games_player_stats_by_seasons_id_fkey",
                        column: x => x.player_stats_by_season_id,
                        principalSchema: "performance",
                        principalTable: "player_stats_by_seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_player_batting_stats_by_games_player_stats_by_season_id",
                schema: "performance",
                table: "player_batting_stats_by_games",
                column: "player_stats_by_season_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_fielding_stats_by_games_player_stats_by_season_id",
                schema: "performance",
                table: "player_fielding_stats_by_games",
                column: "player_stats_by_season_id");

            migrationBuilder.CreateIndex(
                name: "IX_player_pitching_stats_by_games_player_stats_by_season_id",
                schema: "performance",
                table: "player_pitching_stats_by_games",
                column: "player_stats_by_season_id");

            migrationBuilder.CreateIndex(
                name: "player_stats_by_seasons_season_idx",
                schema: "performance",
                table: "player_stats_by_seasons",
                column: "season")
                .Annotation("Npgsql:IndexMethod", "btree");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_batting_stats_by_games",
                schema: "performance");

            migrationBuilder.DropTable(
                name: "player_fielding_stats_by_games",
                schema: "performance");

            migrationBuilder.DropTable(
                name: "player_pitching_stats_by_games",
                schema: "performance");

            migrationBuilder.DropTable(
                name: "player_stats_by_seasons",
                schema: "performance");
        }
    }
}
