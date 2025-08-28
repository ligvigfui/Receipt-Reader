using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.Data.Migrations
{
    /// <inheritdoc />
    public partial class idc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Owner1",
                table: "Receipts");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Owner",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Receipts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Images",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Receipts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Images",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Owner1",
                table: "Receipts",
                sql: "(UserId IS NOT NULL AND GroupId IS NULL) OR (UserId IS NULL AND GroupId IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Owner",
                table: "Images",
                sql: "(UserId IS NOT NULL AND GroupId IS NULL) OR (UserId IS NULL AND GroupId IS NOT NULL)");
        }
    }
}
