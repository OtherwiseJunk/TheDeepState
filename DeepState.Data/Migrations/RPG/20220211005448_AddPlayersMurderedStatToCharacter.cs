using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.RPG
{
    public partial class AddPlayersMurderedStatToCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayersMurdered",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayersMurdered",
                table: "Characters");
        }
    }
}
