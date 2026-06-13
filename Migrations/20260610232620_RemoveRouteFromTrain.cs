using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayCateringERPSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRouteFromTrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Route",
                table: "Trains");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "Trains",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
