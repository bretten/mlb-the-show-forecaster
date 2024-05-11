using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class InitialCreatePlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "players");

            migrationBuilder.CreateTable(
                name: "players",
                schema: "players",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mlb_id = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    birthdate = table.Column<DateTime>(type: "date", nullable: false),
                    position = table.Column<string>(type: "varchar(4)", nullable: false),
                    mlb_debut_date = table.Column<DateTime>(type: "date", nullable: false),
                    bat_side = table.Column<string>(type: "varchar(4)", nullable: false),
                    throw_arm = table.Column<string>(type: "varchar(4)", nullable: false),
                    team = table.Column<string>(type: "varchar(4)", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("players_pkey", x => x.id);
                    table.UniqueConstraint("players_mlb_id_key", x => x.mlb_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "players",
                schema: "players");
        }
    }
}
