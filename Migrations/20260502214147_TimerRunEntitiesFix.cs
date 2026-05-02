using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timer.Migrations
{
    /// <inheritdoc />
    public partial class TimerRunEntitiesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RunEntities_RunEndTime",
                table: "RunEntities",
                column: "RunEndTime",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RunEntities_RunEndTime",
                table: "RunEntities");
        }
    }
}
