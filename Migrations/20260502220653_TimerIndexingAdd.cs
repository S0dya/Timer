using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timer.Migrations
{
    /// <inheritdoc />
    public partial class TimerIndexingAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RunEntities_RunEndTime",
                table: "RunEntities");

            migrationBuilder.CreateIndex(
                name: "IX_RunEntities_UserId",
                table: "RunEntities",
                column: "UserId",
                unique: true,
                filter: "\"RunEndTime\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RunEntities_UserId",
                table: "RunEntities");

            migrationBuilder.CreateIndex(
                name: "IX_RunEntities_RunEndTime",
                table: "RunEntities",
                column: "RunEndTime",
                descending: new bool[0]);
        }
    }
}
