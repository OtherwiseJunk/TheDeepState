using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.RPG
{
    public partial class addForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_characterDiscordUserId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_characterDiscordUserId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "characterDiscordUserId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "characterDiscordUserId",
                table: "Items",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_characterDiscordUserId",
                table: "Items",
                column: "characterDiscordUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_characterDiscordUserId",
                table: "Items",
                column: "characterDiscordUserId",
                principalTable: "Characters",
                principalColumn: "DiscordUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
