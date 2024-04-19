using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class JobPostEmployerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "JobPostings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_EmployerId",
                table: "JobPostings",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Employers_EmployerId",
                table: "JobPostings",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Employers_EmployerId",
                table: "JobPostings");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_EmployerId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "JobPostings");
        }
    }
}
