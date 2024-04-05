using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class timeentry_approval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Timesheets_TimesheetId",
                table: "TimeEntries");

            migrationBuilder.AlterColumn<int>(
                name: "TimesheetId",
                table: "TimeEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalCategory",
                table: "TimeEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Timesheets_TimesheetId",
                table: "TimeEntries",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Timesheets_TimesheetId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ApprovalCategory",
                table: "TimeEntries");

            migrationBuilder.AlterColumn<int>(
                name: "TimesheetId",
                table: "TimeEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Timesheets_TimesheetId",
                table: "TimeEntries",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id");
        }
    }
}
