using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CITPracticum.Data.Migrations
{
    public partial class UpdatingEmployerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SVName",
                table: "FormAs",
                newName: "SVLastName");

            migrationBuilder.AddColumn<string>(
                name: "SVFirstName",
                table: "FormAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "Affiliation",
                table: "Employers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Employers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CredOther",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Credentials",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrgType",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SVPosition",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employers_AddressId",
                table: "Employers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employers_Addresses_AddressId",
                table: "Employers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employers_Addresses_AddressId",
                table: "Employers");

            migrationBuilder.DropIndex(
                name: "IX_Employers_AddressId",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "SVFirstName",
                table: "FormAs");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "CredOther",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Credentials",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "OrgType",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "SVPosition",
                table: "Employers");

            migrationBuilder.RenameColumn(
                name: "SVLastName",
                table: "FormAs",
                newName: "SVName");

            migrationBuilder.AlterColumn<bool>(
                name: "Affiliation",
                table: "Employers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
