using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class FormAtablecomplete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SVCredentialsViewModel");

            migrationBuilder.AddColumn<string>(
                name: "SVCredentials",
                table: "FormAs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SVCredentials",
                table: "FormAs");

            migrationBuilder.CreateTable(
                name: "SVCredentialsViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormAId = table.Column<int>(type: "int", nullable: true),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SVCredentialsViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SVCredentialsViewModel_FormAs_FormAId",
                        column: x => x.FormAId,
                        principalTable: "FormAs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SVCredentialsViewModel_FormAId",
                table: "SVCredentialsViewModel",
                column: "FormAId");
        }
    }
}
