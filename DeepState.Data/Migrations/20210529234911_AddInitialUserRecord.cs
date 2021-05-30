using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations
{
    public partial class AddInitialUserRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRecords",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    DiscordGuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    LibcraftCoinBalance = table.Column<double>(type: "float", nullable: false),
                    TableFlipCount = table.Column<int>(type: "int", nullable: false),
                    TimeOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecords", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRecords");
        }
    }
}
