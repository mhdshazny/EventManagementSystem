using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagementSystem.Migrations
{
    public partial class ChangeInEventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventRecordHanledBy",
                table: "tbl_Events");

            migrationBuilder.AddColumn<string>(
                name: "EventRecordHanledByID",
                table: "tbl_Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Events_EventRecordHanledByID",
                table: "tbl_Events",
                column: "EventRecordHanledByID");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Events_Tbl_Employee_EventRecordHanledByID",
                table: "tbl_Events",
                column: "EventRecordHanledByID",
                principalTable: "Tbl_Employee",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Events_Tbl_Employee_EventRecordHanledByID",
                table: "tbl_Events");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Events_EventRecordHanledByID",
                table: "tbl_Events");

            migrationBuilder.DropColumn(
                name: "EventRecordHanledByID",
                table: "tbl_Events");

            migrationBuilder.AddColumn<string>(
                name: "EventRecordHanledBy",
                table: "tbl_Events",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
