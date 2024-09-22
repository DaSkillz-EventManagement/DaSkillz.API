using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuizModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisedEvents",
                columns: table => new
                {
                    PurchaserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<long>(type: "bigint", nullable: false),
                    PurchasedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisedEvents", x => new { x.PurchaserId, x.EventId });
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false),
                    ExpiredDate = table.Column<long>(type: "bigint", nullable: false),
                    NOAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "Number of attempts allowed for the coupon"),
                    DiscountType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Type of discount: 'Money' or 'Percent'"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Discount value, depends on DiscountType (money or percentage)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
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
                name: "CouponUser",
                columns: table => new
                {
                    CouponsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUser", x => new { x.CouponsId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_CouponUser_Coupons_CouponsId",
                        column: x => x.CouponsId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponUser_User_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    Theme = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CreatedByNavigationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdvertisedEventPurchaserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdvertisedEventEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AdvertisedEvents_AdvertisedEventPurchaserId_AdvertisedEventEventId",
                        columns: x => new { x.AdvertisedEventPurchaserId, x.AdvertisedEventEventId },
                        principalTable: "AdvertisedEvents",
                        principalColumns: new[] { "PurchaserId", "EventId" });
                    table.ForeignKey(
                        name: "FK_Events_User_CreatedByNavigationUserId",
                        column: x => x.CreatedByNavigationUserId,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Price_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscription_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponEvent",
                columns: table => new
                {
                    CouponsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponEvent", x => new { x.CouponsId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_CouponEvent_Coupons_CouponsId",
                        column: x => x.CouponsId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__001C802BACDA8C5B", x => new { x.UserID, x.EventID });
                    table.ForeignKey(
                        name: "FK__Feedback__EventI__4E88ABD4",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Feedback__UserID__4D94879B",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
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
                name: "Quiz",
                columns: table => new
                {
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    eventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuizDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quiz_Events_eventId",
                        column: x => x.eventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quiz_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SponsorEvent",
                columns: table => new
                {
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsSponsored = table.Column<bool>(type: "bit", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(19,2)", nullable: true),
                    SponsorType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SponsorEvent", x => new { x.EventID, x.UserID });
                    table.ForeignKey(
                        name: "FK__SponsorEv__Event__5FB337D6",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__SponsorEv__UserI__60A75C0F",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Apptransid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Zptransid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSubscription = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Apptransid);
                    table.ForeignKey(
                        name: "FK_Transaction_Event",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsMultipleAnswers = table.Column<bool>(type: "bit", nullable: false),
                    IsQuestionAnswered = table.Column<bool>(type: "bit", nullable: false),
                    CorrectAnswerLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowAnswerAfterChoosing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Quiz_Questions",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "QuizId");
                });

            migrationBuilder.CreateTable(
                name: "RefundTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    refundId = table.Column<long>(type: "bigint", maxLength: 100, nullable: false),
                    returnCode = table.Column<int>(type: "int", nullable: false),
                    returnMessage = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    refundAmount = table.Column<long>(type: "bigint", nullable: false),
                    refundAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Apptransid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundTransaction_Transaction",
                        column: x => x.Apptransid,
                        principalTable: "Transactions",
                        principalColumn: "Apptransid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerLabel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsCorrectAnswer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answer_Question_111022",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswer",
                columns: table => new
                {
                    UserAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerLabel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswer", x => x.UserAnswerId);
                    table.ForeignKey(
                        name: "FK_UserAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_UserAnswer_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "QuizId");
                    table.ForeignKey(
                        name: "FK_UserAnswer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponEvent_EventsId",
                table: "CouponEvent",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponUser_UsersUserId",
                table: "CouponUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogo_EventID",
                table: "EventLogo",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_AdvertisedEventPurchaserId_AdvertisedEventEventId",
                table: "Events",
                columns: new[] { "AdvertisedEventPurchaserId", "AdvertisedEventEventId" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedByNavigationUserId",
                table: "Events",
                column: "CreatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTag_EventID",
                table: "EventTag",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_EventID",
                table: "Feedback",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_EventID",
                table: "Participant",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_RoleEventID",
                table: "Participant",
                column: "RoleEventID");

            migrationBuilder.CreateIndex(
                name: "IX_Price_CreatedBy",
                table: "Price",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuizId",
                table: "Question",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_eventId",
                table: "Quiz",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_UserId",
                table: "Quiz",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RefundTransactions_Apptransid",
                table: "RefundTransactions",
                column: "Apptransid",
                unique: true,
                filter: "[Apptransid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorEvent_UserID",
                table: "SponsorEvent",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_EventId",
                table: "Transactions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_QuestionId",
                table: "UserAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_QuizId",
                table: "UserAnswer",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_UserId",
                table: "UserAnswer",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "CouponEvent");

            migrationBuilder.DropTable(
                name: "CouponUser");

            migrationBuilder.DropTable(
                name: "EventLogo");

            migrationBuilder.DropTable(
                name: "EventTag");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RefundTransactions");

            migrationBuilder.DropTable(
                name: "SponsorEvent");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserAnswer");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Logo");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "RoleEvent");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "AdvertisedEvents");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
