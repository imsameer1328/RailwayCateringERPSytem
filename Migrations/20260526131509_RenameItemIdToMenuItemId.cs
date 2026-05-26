using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailwayCateringERPSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameItemIdToMenuItemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemIngredients_MenuItems_ItemId",
                table: "MenuItemIngredients");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "MenuItemIngredients",
                newName: "MenuItemId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItemIngredients_ItemId",
                table: "MenuItemIngredients",
                newName: "IX_MenuItemIngredients_MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemIngredients_MenuItems_MenuItemId",
                table: "MenuItemIngredients",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemIngredients_MenuItems_MenuItemId",
                table: "MenuItemIngredients");

            migrationBuilder.RenameColumn(
                name: "MenuItemId",
                table: "MenuItemIngredients",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItemIngredients_MenuItemId",
                table: "MenuItemIngredients",
                newName: "IX_MenuItemIngredients_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemIngredients_MenuItems_ItemId",
                table: "MenuItemIngredients",
                column: "ItemId",
                principalTable: "MenuItems",
                principalColumn: "MenuItemId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
