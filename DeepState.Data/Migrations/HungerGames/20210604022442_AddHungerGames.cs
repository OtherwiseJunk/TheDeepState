using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.HungerGames
{
    public partial class AddHungerGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrizePools",
                columns: table => new
                {
                    PrizePoolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordGuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    PrizePool = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizePools", x => x.PrizePoolID);
                });

            migrationBuilder.CreateTable(
                name: "Tributes",
                columns: table => new
                {
                    TributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordGuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    DiscordUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tributes", x => x.TributeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrizePools");

            migrationBuilder.DropTable(
                name: "Tributes");
        }
    }
}
