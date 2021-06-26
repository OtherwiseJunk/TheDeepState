using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.ModTeamRequest
{
    public partial class AddModTeamRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 6, 26, 9, 9, 33, 147, DateTimeKind.Local).AddTicks(3246)),
                    UpdateDatetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestingUserDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    DiscordGuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Price = table.Column<double>(type: "float", nullable: true),
                    modifyingModDiscordId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ClosingMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
