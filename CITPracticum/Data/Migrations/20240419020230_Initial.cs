using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<int>(
                name: "AdministratorId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverLetter = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracSV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Org = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PracSVComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SVSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SVSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    StuComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    InsComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    PracSV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Org = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    B8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    C12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PracSVComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SVSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SVSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    StuComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    InsComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDs", x => x.Id);
                });

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
                    StuSign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuSignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false),
                    Submitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFOIPs", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "JobPostings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentCategory = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Archived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPostings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timesheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SVPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Credentials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    Affiliation = table.Column<bool>(type: "bit", nullable: true),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    SVFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SVCredentials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SVCredOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentCategory = table.Column<int>(type: "int", nullable: false),
                    OutOfCountry = table.Column<int>(type: "int", nullable: false),
                    Submitted = table.Column<bool>(type: "bit", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false)
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
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrgName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracSV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StuSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuSignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpSignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    StuLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgStartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PracStartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollegeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    Submitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStuInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormStuInfos_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StuEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    JobPostingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_JobPostings_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPostings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoursToDate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalCategory = table.Column<int>(type: "int", nullable: false),
                    TimesheetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Timesheets_TimesheetId",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheets",
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
                    FormStuInfoId = table.Column<int>(type: "int", nullable: true),
                    FormExitInterviewId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_PracticumForms_FormExitInterviews_FormExitInterviewId",
                        column: x => x.FormExitInterviewId,
                        principalTable: "FormExitInterviews",
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

            migrationBuilder.CreateTable(
                name: "Placements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    InstructorId = table.Column<int>(type: "int", nullable: true),
                    EmployerId = table.Column<int>(type: "int", nullable: true),
                    PracticumFormsId = table.Column<int>(type: "int", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    JobPostingId = table.Column<int>(type: "int", nullable: true),
                    TimesheetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Placements_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Placements_Employers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Placements_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Placements_JobPostings_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPostings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Placements_PracticumForms_PracticumFormsId",
                        column: x => x.PracticumFormsId,
                        principalTable: "PracticumForms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Placements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Placements_Timesheets_TimesheetId",
                        column: x => x.TimesheetId,
                        principalTable: "Timesheets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdministratorId",
                table: "AspNetUsers",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployerId",
                table: "AspNetUsers",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InstructorId",
                table: "AspNetUsers",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StudentId",
                table: "AspNetUsers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_AddressId",
                table: "Employers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_AppUserId",
                table: "Employers",
                column: "AppUserId");

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
                name: "IX_Placements_DocumentId",
                table: "Placements",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_EmployerId",
                table: "Placements",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Placements_InstructorId",
                table: "Placements",
                column: "InstructorId");

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
                name: "IX_PracticumForms_FormExitInterviewId",
                table: "PracticumForms",
                column: "FormExitInterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormFOIPId",
                table: "PracticumForms",
                column: "FormFOIPId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticumForms_FormStuInfoId",
                table: "PracticumForms",
                column: "FormStuInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AppUserId",
                table: "Students",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DocumentId",
                table: "Students",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_JobPostingId",
                table: "Students",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_TimesheetId",
                table: "TimeEntries",
                column: "TimesheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Administrator_AdministratorId",
                table: "AspNetUsers",
                column: "AdministratorId",
                principalTable: "Administrator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employers_EmployerId",
                table: "AspNetUsers",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Instructors_InstructorId",
                table: "AspNetUsers",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_StudentId",
                table: "AspNetUsers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Administrator_AdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employers_EmployerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Instructors_InstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_StudentId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Placements");

            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "PracticumForms");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Timesheets");

            migrationBuilder.DropTable(
                name: "FormAs");

            migrationBuilder.DropTable(
                name: "FormBs");

            migrationBuilder.DropTable(
                name: "FormCs");

            migrationBuilder.DropTable(
                name: "FormDs");

            migrationBuilder.DropTable(
                name: "FormExitInterviews");

            migrationBuilder.DropTable(
                name: "FormFOIPs");

            migrationBuilder.DropTable(
                name: "FormStuInfos");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "JobPostings");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StudentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdministratorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
