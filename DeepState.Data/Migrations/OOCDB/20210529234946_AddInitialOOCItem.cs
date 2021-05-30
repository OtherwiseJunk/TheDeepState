using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.OOCDB
{
    public partial class AddInitialOOCItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutOfContextRecords",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportingUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Base64Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateStored = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutOfContextRecords", x => x.ItemID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutOfContextRecords");
        }
    }
}
