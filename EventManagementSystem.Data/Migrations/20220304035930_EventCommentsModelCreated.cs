using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagementSystem.Migrations
{
    public partial class EventCommentsModelCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_EventComments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CommentedUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_EventComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tbl_EventComments_Tbl_Employee_CommentedUserID",
                        column: x => x.CommentedUserID,
                        principalTable: "Tbl_Employee",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_EventComments_tbl_Events_EventID1",
                        column: x => x.EventID1,
                        principalTable: "tbl_Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_EventComments_CommentedUserID",
                table: "Tbl_EventComments",
                column: "CommentedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_EventComments_EventID1",
                table: "Tbl_EventComments",
                column: "EventID1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_EventComments");
        }
    }
}
