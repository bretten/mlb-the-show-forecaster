﻿using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class InitialCreateListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "game_cards");

            migrationBuilder.CreateTable(
                name: "listings",
                schema: "game_cards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    card_external_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buy_price = table.Column<int>(type: "integer", nullable: false),
                    sell_price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("listings_pkey", x => x.id);
                    table.UniqueConstraint("listings_year_card_external_id_key", x => new { x.year, x.card_external_id });
                });

            migrationBuilder.CreateTable(
                name: "listing_historical_prices",
                schema: "game_cards",
                columns: table => new
                {
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    buy_price = table.Column<int>(type: "integer", nullable: false),
                    sell_price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("listing_historical_prices_pkey", x => new { x.listing_id, x.date });
                    table.ForeignKey(
                        name: "listing_historical_prices_listings_id_fkey",
                        column: x => x.listing_id,
                        principalSchema: "game_cards",
                        principalTable: "listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_orders",
                schema: "game_cards",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("listing_orders_pkey", x => x.id);
                    table.ForeignKey(
                        name: "listing_orders_listings_id_fkey",
                        column: x => x.listing_id,
                        principalSchema: "game_cards",
                        principalTable: "listings",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "listing_historical_prices_listing_id_idx",
                schema: "game_cards",
                table: "listing_historical_prices",
                column: "listing_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "listing_orders_listing_id_idx",
                schema: "game_cards",
                table: "listing_orders",
                column: "listing_id")
                .Annotation("Npgsql:IndexMethod", "btree");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "listing_historical_prices",
                schema: "game_cards");

            migrationBuilder.DropTable(
                name: "listing_orders",
                schema: "game_cards");

            migrationBuilder.DropTable(
                name: "listings",
                schema: "game_cards");
        }
    }
}
