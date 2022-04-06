using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.OOCDB
{
    public partial class AddGuildIdToOOC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Base64Image",
                table: "OutOfContextRecords",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscordGuildId",
                table: "OutOfContextRecords",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordGuildId",
                table: "OutOfContextRecords");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "OutOfContextRecords",
                newName: "Base64Image");
        }
    }
}
