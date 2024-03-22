using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class UpdatingDocumentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Document_DocumentId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Employers_EmployerId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_JobPostings_JobPostingId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_PracticumForms_PracticumFormsId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Students_StudentId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Timesheets_TimesheetId",
                table: "Placements");

            migrationBuilder.AlterColumn<int>(
                name: "TimesheetId",
                table: "Placements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Placements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PracticumFormsId",
                table: "Placements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JobPostingId",
                table: "Placements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerId",
                table: "Placements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "Placements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Document_DocumentId",
                table: "Placements",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Employers_EmployerId",
                table: "Placements",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_JobPostings_JobPostingId",
                table: "Placements",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_PracticumForms_PracticumFormsId",
                table: "Placements",
                column: "PracticumFormsId",
                principalTable: "PracticumForms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Students_StudentId",
                table: "Placements",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Timesheets_TimesheetId",
                table: "Placements",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Document_DocumentId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Employers_EmployerId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_JobPostings_JobPostingId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_PracticumForms_PracticumFormsId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Students_StudentId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Timesheets_TimesheetId",
                table: "Placements");

            migrationBuilder.AlterColumn<int>(
                name: "TimesheetId",
                table: "Placements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Placements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PracticumFormsId",
                table: "Placements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobPostingId",
                table: "Placements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployerId",
                table: "Placements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DocumentId",
                table: "Placements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Document_DocumentId",
                table: "Placements",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Employers_EmployerId",
                table: "Placements",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_JobPostings_JobPostingId",
                table: "Placements",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_PracticumForms_PracticumFormsId",
                table: "Placements",
                column: "PracticumFormsId",
                principalTable: "PracticumForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Students_StudentId",
                table: "Placements",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placements_Timesheets_TimesheetId",
                table: "Placements",
                column: "TimesheetId",
                principalTable: "Timesheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
