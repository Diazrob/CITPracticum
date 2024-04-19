using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class employercomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployerComments",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerComments",
                table: "Employers");
        }
    }
}
