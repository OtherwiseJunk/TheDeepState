using Microsoft.EntityFrameworkCore.Migrations;

namespace DeepState.Data.Migrations.HungerGames
{
    public partial class AddAdditionalChannelAnnouncement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnnouncementChannelId",
                table: "GuildConfigurations",
                newName: "TributeAnnouncementChannelId");

            migrationBuilder.AddColumn<decimal>(
                name: "CorpseAnnouncementChannelId",
                table: "GuildConfigurations",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorpseAnnouncementChannelId",
                table: "GuildConfigurations");

            migrationBuilder.RenameColumn(
                name: "TributeAnnouncementChannelId",
                table: "GuildConfigurations",
                newName: "AnnouncementChannelId");
        }
    }
}
