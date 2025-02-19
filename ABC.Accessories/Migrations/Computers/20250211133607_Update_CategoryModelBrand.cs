using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ABC.Accessories.Migrations.Computers
{
    /// <inheritdoc />
    public partial class Update_CategoryModelBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropColumn(
                name: "Category",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "abc-computers",
                table: "Category",
                newName: "Source");

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                schema: "abc-computers",
                table: "Category",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "abc-computers",
                table: "Category",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                schema: "abc-computers",
                table: "AccessoryBase",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "abc-computers",
                table: "AccessoryBase",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeviceModelId",
                schema: "abc-computers",
                table: "AccessoryBase",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DeviceModel",
                schema: "abc-computers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AltText = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryBase_BrandId",
                schema: "abc-computers",
                table: "AccessoryBase",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryBase_CategoryId",
                schema: "abc-computers",
                table: "AccessoryBase",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryBase_DeviceModelId",
                schema: "abc-computers",
                table: "AccessoryBase",
                column: "DeviceModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryBase_Brands_BrandId",
                schema: "abc-computers",
                table: "AccessoryBase",
                column: "BrandId",
                principalSchema: "abc-computers",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryBase_Category_CategoryId",
                schema: "abc-computers",
                table: "AccessoryBase",
                column: "CategoryId",
                principalSchema: "abc-computers",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessoryBase_DeviceModel_DeviceModelId",
                schema: "abc-computers",
                table: "AccessoryBase",
                column: "DeviceModelId",
                principalSchema: "abc-computers",
                principalTable: "DeviceModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryBase_Brands_BrandId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryBase_Category_CategoryId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessoryBase_DeviceModel_DeviceModelId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropTable(
                name: "DeviceModel",
                schema: "abc-computers");

            migrationBuilder.DropIndex(
                name: "IX_AccessoryBase_BrandId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropIndex(
                name: "IX_AccessoryBase_CategoryId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropIndex(
                name: "IX_AccessoryBase_DeviceModelId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropColumn(
                name: "AltText",
                schema: "abc-computers",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "abc-computers",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BrandId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.DropColumn(
                name: "DeviceModelId",
                schema: "abc-computers",
                table: "AccessoryBase");

            migrationBuilder.RenameColumn(
                name: "Source",
                schema: "abc-computers",
                table: "Category",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                schema: "abc-computers",
                table: "AccessoryBase",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "abc-computers",
                table: "AccessoryBase",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                schema: "abc-computers",
                table: "AccessoryBase",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
