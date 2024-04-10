using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class ExitInterviewForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormExitInterviewId",
                table: "PracticumForms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormExitInterviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A6 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormExitInterviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormExitInterviewId",
                table: "PracticumForms",
                column: "FormExitInterviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_PracticumForms_FormExitInterviews_FormExitInterviewId",
                table: "PracticumForms",
                column: "FormExitInterviewId",
                principalTable: "FormExitInterviews",
                principalColumn: "Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PracticumForms_FormExitInterviews_FormExitInterviewId",
                table: "PracticumForms");

            migrationBuilder.DropTable(
                name: "FormExitInterviews");

            migrationBuilder.DropIndex(
                name: "IX_PracticumForms_FormExitInterviewId",
                table: "PracticumForms");

            migrationBuilder.DropColumn(
                name: "FormExitInterviewId",
                table: "PracticumForms");

        }
    }
}
