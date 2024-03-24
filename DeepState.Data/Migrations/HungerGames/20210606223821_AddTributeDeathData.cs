using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.HungerGames
{
    public partial class AddTributeDeathData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeathMessage",
                table: "Tributes",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "District",
                table: "Tributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlive",
                table: "Tributes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ObituaryMessage",
                table: "Tributes",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeathMessage",
                table: "Tributes");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Tributes");

            migrationBuilder.DropColumn(
                name: "IsAlive",
                table: "Tributes");

            migrationBuilder.DropColumn(
                name: "ObituaryMessage",
                table: "Tributes");
        }
    }
}
