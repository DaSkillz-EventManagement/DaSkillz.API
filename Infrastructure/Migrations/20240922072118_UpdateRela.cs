using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AdvertisedEvents_AdvertisedEventPurchaserId_AdvertisedEventEventId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AdvertisedEventPurchaserId_AdvertisedEventEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdvertisedEventEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AdvertisedEventPurchaserId",
                table: "Events");

            migrationBuilder.AddColumn<long>(
                name: "CreatedDate",
                table: "AdvertisedEvents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisedEvents_EventId",
                table: "AdvertisedEvents",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisedEvents_Events_EventId",
                table: "AdvertisedEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisedEvents_Events_EventId",
                table: "AdvertisedEvents");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisedEvents_EventId",
                table: "AdvertisedEvents");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AdvertisedEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "AdvertisedEventEventId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdvertisedEventPurchaserId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdvertisedEventPurchaserId_AdvertisedEventEventId",
                table: "Events",
                columns: new[] { "AdvertisedEventPurchaserId", "AdvertisedEventEventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AdvertisedEvents_AdvertisedEventPurchaserId_AdvertisedEventEventId",
                table: "Events",
                columns: new[] { "AdvertisedEventPurchaserId", "AdvertisedEventEventId" },
                principalTable: "AdvertisedEvents",
                principalColumns: new[] { "PurchaserId", "EventId" });
        }
    }
}
