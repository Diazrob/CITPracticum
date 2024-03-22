using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class FormsPart1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employer_AspNetUsers_AppUserId",
                table: "Employer");

            migrationBuilder.DropForeignKey(
                name: "FK_Employer_Student_StudentId",
                table: "Employer");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_AppUserId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Employer_EmployerId",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employer",
                table: "Employer");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "Employer",
                newName: "Employers");

            migrationBuilder.RenameIndex(
                name: "IX_Student_EmployerId",
                table: "Students",
                newName: "IX_Students_EmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_AppUserId",
                table: "Students",
                newName: "IX_Students_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employer_StudentId",
                table: "Employers",
                newName: "IX_Employers_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Employer_AppUserId",
                table: "Employers",
                newName: "IX_Employers_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employers",
                table: "Employers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Org = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracSVComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVSign = table.Column<bool>(type: "bit", nullable: false),
                    SVSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    StuComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuSign = table.Column<bool>(type: "bit", nullable: false),
                    StuSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    InsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsSign = table.Column<bool>(type: "bit", nullable: false),
                    InsSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormCs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormDs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Org = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    A5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    C12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracSVComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVSign = table.Column<bool>(type: "bit", nullable: false),
                    SVSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    StuComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuSign = table.Column<bool>(type: "bit", nullable: false),
                    StuSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    InsComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsSign = table.Column<bool>(type: "bit", nullable: false),
                    InsSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormFOIPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Other = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuSign = table.Column<bool>(type: "bit", nullable: false),
                    StuSignDate = table.Column<DateTime>(type: "date", nullable: false),
                    Submitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFOIPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HostCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrgType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVCredentials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVCredOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    PaymentCategory = table.Column<int>(type: "int", nullable: false),
                    Submitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormAs_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormBs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracHost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuSign = table.Column<bool>(type: "bit", nullable: false),
                    StuSignDate = table.Column<DateTime>(type: "date", nullable: false),
                    EmpSign = table.Column<bool>(type: "bit", nullable: false),
                    EmpSignDate = table.Column<DateTime>(type: "date", nullable: false),
                    Submitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormBs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormBs_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormStuInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgStartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracStartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollegeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AltPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Submitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStuInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormStuInfos_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PracticumForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormAId = table.Column<int>(type: "int", nullable: true),
                    FormBId = table.Column<int>(type: "int", nullable: true),
                    FormCId = table.Column<int>(type: "int", nullable: true),
                    FormDId = table.Column<int>(type: "int", nullable: true),
                    FormFOIPId = table.Column<int>(type: "int", nullable: true),
                    FormStuInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticumForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticumForms_FormAs_FormAId",
                        column: x => x.FormAId,
                        principalTable: "FormAs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracticumForms_FormBs_FormBId",
                        column: x => x.FormBId,
                        principalTable: "FormBs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracticumForms_FormCs_FormCId",
                        column: x => x.FormCId,
                        principalTable: "FormCs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracticumForms_FormDs_FormDId",
                        column: x => x.FormDId,
                        principalTable: "FormDs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracticumForms_FormFOIPs_FormFOIPId",
                        column: x => x.FormFOIPId,
                        principalTable: "FormFOIPs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PracticumForms_FormStuInfos_FormStuInfoId",
                        column: x => x.FormStuInfoId,
                        principalTable: "FormStuInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormAs_AddressId",
                table: "FormAs",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_FormBs_AddressId",
                table: "FormBs",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_FormStuInfos_AddressId",
                table: "FormStuInfos",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormAId",
                table: "PracticumForms",
                column: "FormAId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormBId",
                table: "PracticumForms",
                column: "FormBId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormCId",
                table: "PracticumForms",
                column: "FormCId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormDId",
                table: "PracticumForms",
                column: "FormDId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormFOIPId",
                table: "PracticumForms",
                column: "FormFOIPId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormStuInfoId",
                table: "PracticumForms",
                column: "FormStuInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employers_AspNetUsers_AppUserId",
                table: "Employers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employers_Students_StudentId",
                table: "Employers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_AppUserId",
                table: "Students",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Employers_EmployerId",
                table: "Students",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employers_AspNetUsers_AppUserId",
                table: "Employers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employers_Students_StudentId",
                table: "Employers");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_AppUserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Employers_EmployerId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "PracticumForms");

            migrationBuilder.DropTable(
                name: "FormAs");

            migrationBuilder.DropTable(
                name: "FormBs");

            migrationBuilder.DropTable(
                name: "FormCs");

            migrationBuilder.DropTable(
                name: "FormDs");

            migrationBuilder.DropTable(
                name: "FormFOIPs");

            migrationBuilder.DropTable(
                name: "FormStuInfos");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employers",
                table: "Employers");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "Employers",
                newName: "Employer");

            migrationBuilder.RenameIndex(
                name: "IX_Students_EmployerId",
                table: "Student",
                newName: "IX_Student_EmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_AppUserId",
                table: "Student",
                newName: "IX_Student_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employers_StudentId",
                table: "Employer",
                newName: "IX_Employer_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Employers_AppUserId",
                table: "Employer",
                newName: "IX_Employer_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employer",
                table: "Employer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employer_AspNetUsers_AppUserId",
                table: "Employer",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employer_Student_StudentId",
                table: "Employer",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_AppUserId",
                table: "Student",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Employer_EmployerId",
                table: "Student",
                column: "EmployerId",
                principalTable: "Employer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
