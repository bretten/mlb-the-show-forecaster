﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;

#nullable disable

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(MarketplaceDbContext))]
    [Migration("20240415002152_InitialCreateListings")]
    partial class InitialCreateListings
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

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Listing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<int>("BuyPrice")
                        .HasColumnType("integer")
                        .HasColumnName("buy_price")
                        .HasColumnOrder(2);

                    b.Property<Guid>("CardExternalId")
                        .HasColumnType("uuid")
                        .HasColumnName("card_external_id")
                        .HasColumnOrder(1);

                    b.Property<int>("SellPrice")
                        .HasColumnType("integer")
                        .HasColumnName("sell_price")
                        .HasColumnOrder(3);

                    b.HasKey("Id")
                        .HasName("listings_pkey");

                    b.HasIndex(new[] { "CardExternalId" }, "listings_card_external_id_idx");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex(new[] { "CardExternalId" }, "listings_card_external_id_idx"), "btree");

                    b.ToTable("listings", "game_cards");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects.ListingHistoricalPrice", b =>
                {
                    b.Property<Guid>("listing_id")
                        .HasColumnType("uuid")
                        .HasColumnOrder(0);

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date")
                        .HasColumnOrder(1);

                    b.Property<int>("BuyPrice")
                        .HasColumnType("integer")
                        .HasColumnName("buy_price")
                        .HasColumnOrder(2);

                    b.Property<int>("SellPrice")
                        .HasColumnType("integer")
                        .HasColumnName("sell_price")
                        .HasColumnOrder(3);

                    b.HasKey("listing_id", "Date")
                        .HasName("listing_historical_prices_pkey");

                    b.ToTable("listing_historical_prices", "game_cards");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects.ListingHistoricalPrice", b =>
                {
                    b.HasOne("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Listing", null)
                        .WithMany("_historicalPrices")
                        .HasForeignKey("listing_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("listing_historical_prices_listings_id_fkey");
                });

            modelBuilder.Entity("com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Listing", b =>
                {
                    b.Navigation("_historicalPrices");
                });
#pragma warning restore 612, 618
        }
    }
}
