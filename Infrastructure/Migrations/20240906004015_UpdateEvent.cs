using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    StartDate = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image = table.Column<string>(type: "varchar(5000)", unicode: false, maxLength: 5000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<long>(type: "bigint", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Approval = table.Column<bool>(type: "bit", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(19,2)", nullable: true),
                    LocationUrl = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    LocationCoord = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    LocationID = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    LocationAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Theme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logo",
                columns: table => new
                {
                    LogoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponsorBrand = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logo", x => x.LogoID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "EventLogo",
                columns: table => new
                {
                    LogoID = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventLog__D1B4592A90BF4813", x => new { x.LogoID, x.EventID });
                    table.ForeignKey(
                        name: "FK__EventLogo__Event__5CD6CB2B",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__EventLogo__LogoI__5DCAEF64",
                        column: x => x.LogoID,
                        principalTable: "Logo",
                        principalColumn: "LogoID");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__User__RoleID__398D8EEE",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    RefreshTokenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenID);
                    table.ForeignKey(
                        name: "FK__RefreshTo__UserI__5535A963",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLogo_EventID",
                table: "EventLogo",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogo");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Logo");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
