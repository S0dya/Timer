using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timer.Migrations
{
    /// <inheritdoc />
    public partial class TimerCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "RunEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RunStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RunEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentSessionStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedSessions = table.Column<int>(type: "integer", nullable: false),
                    PlannedSessionsAmount = table.Column<int>(type: "integer", nullable: false),
                    SessionDuration = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimerSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionDuration = table.Column<int>(type: "integer", nullable: false),
                    SessionsAmount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimerSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RunEntities");

            migrationBuilder.DropTable(
                name: "TimerSettings");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
