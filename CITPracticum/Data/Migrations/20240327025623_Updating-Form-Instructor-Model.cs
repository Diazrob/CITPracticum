using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class UpdatingFormInstructorModel : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Placements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "FormAs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Affiliation",
                table: "Employers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstructorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Placements_InstructorId",
                table: "Placements",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InstructorId",
                table: "AspNetUsers",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Instructors_InstructorId",
                table: "AspNetUsers",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

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
                name: "FK_Placements_Instructors_InstructorId",
                table: "Placements",
                column: "InstructorId",
                principalTable: "Instructors",
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
                name: "FK_AspNetUsers_Instructors_InstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Document_DocumentId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Employers_EmployerId",
                table: "Placements");

            migrationBuilder.DropForeignKey(
                name: "FK_Placements_Instructors_InstructorId",
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

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Placements_InstructorId",
                table: "Placements");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Placements");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "FormAs");

            migrationBuilder.DropColumn(
                name: "OutOfCountry",
                table: "FormAs");

            migrationBuilder.DropColumn(
                name: "Affiliation",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "AspNetUsers");

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
