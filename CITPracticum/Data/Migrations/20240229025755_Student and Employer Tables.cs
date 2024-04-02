using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class StudentandEmployerTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Student",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Student_EmployerId",
                table: "Student",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employer_StudentId",
                table: "Employer",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employer_Student_StudentId",
                table: "Employer",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Employer_EmployerId",
                table: "Student",
                column: "EmployerId",
                principalTable: "Employer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employer_Student_StudentId",
                table: "Employer");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Employer_EmployerId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_EmployerId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Employer_StudentId",
                table: "Employer");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Student");
        }
    }
    }
