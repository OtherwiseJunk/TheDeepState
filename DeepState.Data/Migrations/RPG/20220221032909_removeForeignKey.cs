using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.RPG
{
    public partial class removeForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CharacterId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Items");

            migrationBuilder.AddColumn<decimal>(
                name: "CharacterDiscordUserId",
                table: "Items",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CharacterDiscordUserId",
                table: "Items",
                column: "CharacterDiscordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterDiscordUserId",
                table: "Items",
                column: "CharacterDiscordUserId",
                principalTable: "Characters",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterDiscordUserId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CharacterDiscordUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CharacterDiscordUserId",
                table: "Items");

            migrationBuilder.AddColumn<decimal>(
                name: "CharacterId",
                table: "Items",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CharacterId",
                table: "Items",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
