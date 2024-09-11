using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByNavigationUserId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleEvent",
                columns: table => new
                {
                    RoleEventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleEventName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleEvent", x => x.RoleEventID);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleEventID = table.Column<int>(type: "int", nullable: true),
                    CheckedIn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCheckedMail = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => new { x.UserID, x.EventID });
                    table.ForeignKey(
                        name: "FK_Participant_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Participant_RoleEvent_RoleEventID",
                        column: x => x.RoleEventID,
                        principalTable: "RoleEvent",
                        principalColumn: "RoleEventID");
                    table.ForeignKey(
                        name: "FK_Participant_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "EventTag",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventTag__72E8B6CB39612262", x => new { x.TagID, x.EventID });
                    table.ForeignKey(
                        name: "FK__EventTag__EventI__4316F928",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__EventTag__TagID__440B1D61",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "TagID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedByNavigationUserId",
                table: "Events",
                column: "CreatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_EventID",
                table: "EventTag",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_EventID",
                table: "Participant",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_RoleEventID",
                table: "Participant",
                column: "RoleEventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_User_CreatedByNavigationUserId",
                table: "Events",
                column: "CreatedByNavigationUserId",
                principalTable: "User",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_User_CreatedByNavigationUserId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventTag");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "RoleEvent");

            migrationBuilder.DropIndex(
                name: "IX_Events_CreatedByNavigationUserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationUserId",
                table: "Events");
        }
    }
}
