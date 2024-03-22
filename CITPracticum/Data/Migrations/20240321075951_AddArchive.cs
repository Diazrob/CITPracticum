using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class AddArchive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobPostingId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "JobPostings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Students_JobPostingId",
                table: "Students",
                column: "JobPostingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_JobPostings_JobPostingId",
                table: "Students",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_JobPostings_JobPostingId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_JobPostingId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "JobPostingId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "JobPostings");
        }
    }
}
