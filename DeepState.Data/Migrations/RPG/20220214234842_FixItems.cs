using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.RPG
{
    public partial class FixItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterDiscordUserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CharacterDiscordUserId",
                table: "Items",
                newName: "characterDiscordUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CharacterDiscordUserId",
                table: "Items",
                newName: "IX_Items_characterDiscordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_characterDiscordUserId",
                table: "Items",
                column: "characterDiscordUserId",
                principalTable: "Characters",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_characterDiscordUserId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "characterDiscordUserId",
                table: "Items",
                newName: "CharacterDiscordUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_characterDiscordUserId",
                table: "Items",
                newName: "IX_Items_CharacterDiscordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterDiscordUserId",
                table: "Items",
                column: "CharacterDiscordUserId",
                principalTable: "Characters",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
