using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore.Migrations
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

            migrationBuilder.Sql(Constants.Collations.AccentInsensitiveDefinition);

            migrationBuilder.CreateTable(
                name: "players",
                schema: "players",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mlb_id = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false, collation: "accent_insensitive"),
                    last_name = table.Column<string>(type: "text", nullable: false, collation: "accent_insensitive"),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    position = table.Column<string>(type: "varchar(4)", nullable: false),
                    mlb_debut_date = table.Column<DateOnly>(type: "date", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "players_first_name_last_name_idx",
                schema: "players",
                table: "players",
                columns: new[] { "first_name", "last_name" })
                .Annotation("Npgsql:IndexMethod", "btree")
                .Annotation("Relational:Collation", new[] { "accent_insensitive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "players",
                schema: "players");

            migrationBuilder.Sql($"DROP COLLATION IF EXISTS {Constants.Collations.AccentInsensitive}");
        }
    }
}
