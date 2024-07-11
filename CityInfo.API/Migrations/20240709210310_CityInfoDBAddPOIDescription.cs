using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class CityInfoDBAddPOIDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterests_Cities_Id",
                table: "PointOfInterests");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PointOfInterests",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterests_CityId",
                table: "PointOfInterests",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterests_Cities_CityId",
                table: "PointOfInterests",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterests_Cities_CityId",
                table: "PointOfInterests");

            migrationBuilder.DropIndex(
                name: "IX_PointOfInterests_CityId",
                table: "PointOfInterests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PointOfInterests");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterests_Cities_Id",
                table: "PointOfInterests",
                column: "Id",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
