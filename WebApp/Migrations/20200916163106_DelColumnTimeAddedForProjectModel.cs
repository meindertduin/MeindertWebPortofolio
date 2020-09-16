using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class DelColumnTimeAddedForProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Projects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
