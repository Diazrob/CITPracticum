using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class TimesheetAndPlacements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Timesheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Placements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    EmployerId = table.Column<int>(type: "int", nullable: false),
                    PracticumFormsId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    JobPostingId = table.Column<int>(type: "int", nullable: false),
                    TimesheetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Placements_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_Employers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_JobPostings_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPostings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_PracticumForms_PracticumFormsId",
                        column: x => x.PracticumFormsId,
                        principalTable: "PracticumForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Placements_Timesheets_TimesheetId",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Placements_DocumentId",
                table: "Placements",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_EmployerId",
                table: "Placements",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_JobPostingId",
                table: "Placements",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_PracticumFormsId",
                table: "Placements",
                column: "PracticumFormsId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_StudentId",
                table: "Placements",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_TimesheetId",
                table: "Placements",
                column: "TimesheetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Placements");

            migrationBuilder.DropTable(
                name: "Timesheets");
        }
    }
}
