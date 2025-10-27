using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.Data.Migrations
{
    /// <inheritdoc />
    public partial class measurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAliases",
                table: "ProductAliases");

            migrationBuilder.RenameColumn(
                name: "Measurement",
                table: "ReceiptItems",
                newName: "MeasurementId");

            migrationBuilder.RenameColumn(
                name: "Measurement",
                table: "Products",
                newName: "MeasurementId");

            migrationBuilder.AddColumn<int>(
                name: "ProductAliasId",
                table: "ReceiptItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductDBOId",
                table: "ReceiptItems",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductAliases",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductAliases",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAliases",
                table: "ProductAliases",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plural = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ConversionFactorToSI = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_MeasurementId",
                table: "ReceiptItems",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ProductAliasId",
                table: "ReceiptItems",
                column: "ProductAliasId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ProductDBOId",
                table: "ReceiptItems",
                column: "ProductDBOId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MeasurementId",
                table: "Products",
                column: "MeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Measurements_MeasurementId",
                table: "Products",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_Measurements_MeasurementId",
                table: "ReceiptItems",
                column: "MeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_ProductAliases_ProductAliasId",
                table: "ReceiptItems",
                column: "ProductAliasId",
                principalTable: "ProductAliases",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptItems_Products_ProductDBOId",
                table: "ReceiptItems",
                column: "ProductDBOId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Measurements_MeasurementId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_Measurements_MeasurementId",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_ProductAliases_ProductAliasId",
                table: "ReceiptItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptItems_Products_ProductDBOId",
                table: "ReceiptItems");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptItems_MeasurementId",
                table: "ReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptItems_ProductAliasId",
                table: "ReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptItems_ProductDBOId",
                table: "ReceiptItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_MeasurementId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAliases",
                table: "ProductAliases");

            migrationBuilder.DropColumn(
                name: "ProductAliasId",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "ProductDBOId",
                table: "ReceiptItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductAliases");

            migrationBuilder.RenameColumn(
                name: "MeasurementId",
                table: "ReceiptItems",
                newName: "Measurement");

            migrationBuilder.RenameColumn(
                name: "MeasurementId",
                table: "Products",
                newName: "Measurement");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductAliases",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAliases",
                table: "ProductAliases",
                columns: new[] { "Language", "Name" });
        }
    }
}
