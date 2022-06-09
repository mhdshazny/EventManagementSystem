using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagementSystem.Migrations
{
    public partial class EventTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventRecordHanledBy",
                table: "tbl_Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Visibility",
                table: "tbl_Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventRecordHanledBy",
                table: "tbl_Events");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "tbl_Events");
        }
    }
}
