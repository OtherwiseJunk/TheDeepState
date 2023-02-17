using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeepState.Data.Migrations.BestOf
{
    public partial class BestOfDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BestOfs",
                columns: table => new
                {
                    BestOdIf = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MessageSentDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    TriggeringEmoji = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestOfs", x => x.BestOdIf);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestOfs");
        }
    }
}
