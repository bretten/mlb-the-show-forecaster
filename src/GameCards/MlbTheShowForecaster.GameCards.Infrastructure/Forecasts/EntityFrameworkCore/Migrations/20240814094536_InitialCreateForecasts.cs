using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Forecasts.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class InitialCreateForecasts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "game_cards");

            migrationBuilder.CreateTable(
                name: "player_card_forecasts",
                schema: "game_cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    card_external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    player_mlb_id = table.Column<int>(type: "integer", nullable: true),
                    position = table.Column<string>(type: "varchar(4)", nullable: false),
                    overall_rating = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_card_forecasts_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_card_forecast_impacts",
                schema: "game_cards",
                columns: table => new
                {
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    player_card_forecast_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    boost_reason = table.Column<string>(type: "text", nullable: true),
                    old_overall_rating = table.Column<short>(type: "smallint", nullable: true),
                    new_overall_rating = table.Column<short>(type: "smallint", nullable: true),
                    old_position = table.Column<string>(type: "varchar(4)", nullable: true),
                    new_position = table.Column<string>(type: "varchar(4)", nullable: true),
                    old_score = table.Column<decimal>(type: "numeric(5,4)", nullable: true),
                    new_score = table.Column<decimal>(type: "numeric(5,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_card_forecast_impacts_pkey", x => new { x.player_card_forecast_id, x.type, x.end_date });
                    table.ForeignKey(
                        name: "player_card_forecast_impacts_player_card_forecasts_id_fkey",
                        column: x => x.player_card_forecast_id,
                        principalSchema: "game_cards",
                        principalTable: "player_card_forecasts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "player_card_forecasts_year_card_external_id_idx",
                schema: "game_cards",
                table: "player_card_forecasts",
                columns: new[] { "year", "card_external_id" })
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "player_card_forecasts_year_player_mlb_id_idx",
                schema: "game_cards",
                table: "player_card_forecasts",
                columns: new[] { "year", "player_mlb_id" })
                .Annotation("Npgsql:IndexMethod", "btree");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_card_forecast_impacts",
                schema: "game_cards");

            migrationBuilder.DropTable(
                name: "player_card_forecasts",
                schema: "game_cards");
        }
    }
}
