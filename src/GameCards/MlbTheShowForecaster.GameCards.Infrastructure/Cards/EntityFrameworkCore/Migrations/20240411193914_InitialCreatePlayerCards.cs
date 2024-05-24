using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class InitialCreatePlayerCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "game_cards");

            migrationBuilder.CreateTable(
                name: "player_cards",
                schema: "game_cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "varchar(12)", nullable: false),
                    image_location = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    rarity = table.Column<string>(type: "varchar(8)", nullable: false),
                    series = table.Column<string>(type: "varchar(8)", nullable: false),
                    position = table.Column<string>(type: "varchar(4)", nullable: false),
                    team_short_name = table.Column<string>(type: "varchar(4)", nullable: false),
                    overall_rating = table.Column<short>(type: "smallint", nullable: false),
                    stamina = table.Column<short>(type: "smallint", nullable: false),
                    pitching_clutch = table.Column<short>(type: "smallint", nullable: false),
                    hits_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    strikeouts_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    base_on_balls_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    home_runs_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    pitch_velocity = table.Column<short>(type: "smallint", nullable: false),
                    pitch_control = table.Column<short>(type: "smallint", nullable: false),
                    pitch_movement = table.Column<short>(type: "smallint", nullable: false),
                    contact_left = table.Column<short>(type: "smallint", nullable: false),
                    contact_right = table.Column<short>(type: "smallint", nullable: false),
                    power_left = table.Column<short>(type: "smallint", nullable: false),
                    power_right = table.Column<short>(type: "smallint", nullable: false),
                    plate_vision = table.Column<short>(type: "smallint", nullable: false),
                    plate_discipline = table.Column<short>(type: "smallint", nullable: false),
                    batting_clutch = table.Column<short>(type: "smallint", nullable: false),
                    bunting_ability = table.Column<short>(type: "smallint", nullable: false),
                    drag_bunting_ability = table.Column<short>(type: "smallint", nullable: false),
                    hitting_durability = table.Column<short>(type: "smallint", nullable: false),
                    fielding_durability = table.Column<short>(type: "smallint", nullable: false),
                    fielding_ability = table.Column<short>(type: "smallint", nullable: false),
                    arm_strength = table.Column<short>(type: "smallint", nullable: false),
                    arm_accuracy = table.Column<short>(type: "smallint", nullable: false),
                    reaction_time = table.Column<short>(type: "smallint", nullable: false),
                    blocking = table.Column<short>(type: "smallint", nullable: false),
                    speed = table.Column<short>(type: "smallint", nullable: false),
                    base_running_ability = table.Column<short>(type: "smallint", nullable: false),
                    base_running_aggression = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_cards_pkey", x => x.id);
                    table.UniqueConstraint("player_cards_external_id_key", x => x.external_id);
                });

            migrationBuilder.CreateTable(
                name: "player_card_historical_ratings",
                schema: "game_cards",
                columns: table => new
                {
                    player_card_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    overall_rating = table.Column<short>(type: "smallint", nullable: false),
                    stamina = table.Column<short>(type: "smallint", nullable: false),
                    pitching_clutch = table.Column<short>(type: "smallint", nullable: false),
                    hits_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    strikeouts_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    base_on_balls_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    home_runs_per_nine = table.Column<short>(type: "smallint", nullable: false),
                    pitch_velocity = table.Column<short>(type: "smallint", nullable: false),
                    pitch_control = table.Column<short>(type: "smallint", nullable: false),
                    pitch_movement = table.Column<short>(type: "smallint", nullable: false),
                    contact_left = table.Column<short>(type: "smallint", nullable: false),
                    contact_right = table.Column<short>(type: "smallint", nullable: false),
                    power_left = table.Column<short>(type: "smallint", nullable: false),
                    power_right = table.Column<short>(type: "smallint", nullable: false),
                    plate_vision = table.Column<short>(type: "smallint", nullable: false),
                    plate_discipline = table.Column<short>(type: "smallint", nullable: false),
                    batting_clutch = table.Column<short>(type: "smallint", nullable: false),
                    bunting_ability = table.Column<short>(type: "smallint", nullable: false),
                    drag_bunting_ability = table.Column<short>(type: "smallint", nullable: false),
                    hitting_durability = table.Column<short>(type: "smallint", nullable: false),
                    fielding_durability = table.Column<short>(type: "smallint", nullable: false),
                    fielding_ability = table.Column<short>(type: "smallint", nullable: false),
                    arm_strength = table.Column<short>(type: "smallint", nullable: false),
                    arm_accuracy = table.Column<short>(type: "smallint", nullable: false),
                    reaction_time = table.Column<short>(type: "smallint", nullable: false),
                    blocking = table.Column<short>(type: "smallint", nullable: false),
                    speed = table.Column<short>(type: "smallint", nullable: false),
                    base_running_ability = table.Column<short>(type: "smallint", nullable: false),
                    base_running_aggression = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_card_historical_ratings_pkey", x => new { x.player_card_id, x.start_date, x.end_date });
                    table.ForeignKey(
                        name: "player_card_historical_ratings_player_cards_id_fkey",
                        column: x => x.player_card_id,
                        principalSchema: "game_cards",
                        principalTable: "player_cards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "player_cards_year_external_id_idx",
                schema: "game_cards",
                table: "player_cards",
                columns: new[] { "year", "external_id" })
                .Annotation("Npgsql:IndexMethod", "btree");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_card_historical_ratings",
                schema: "game_cards");

            migrationBuilder.DropTable(
                name: "player_cards",
                schema: "game_cards");
        }
    }
}
