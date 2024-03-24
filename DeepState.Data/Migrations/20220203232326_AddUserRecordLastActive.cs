using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations
{
    public partial class AddUserRecordLastActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimePosted",
                table: "UserRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimePosted",
                table: "UserRecords");
        }
    }
}
