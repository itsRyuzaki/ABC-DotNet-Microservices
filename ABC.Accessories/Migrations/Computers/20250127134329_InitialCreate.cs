using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ABC.Accessories.Migrations.Computers
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "abc");

            migrationBuilder.CreateTable(
                name: "AccessoryBase",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessoryBaseId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    SubCategory = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessoryBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OfficialSite = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MobileNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accessories",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessoryBaseId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SellerPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    AbcPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessories_AccessoryBase_AccessoryBaseId",
                        column: x => x.AccessoryBaseId,
                        principalSchema: "abc",
                        principalTable: "AccessoryBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessorySellerXREF",
                schema: "abc",
                columns: table => new
                {
                    AccessoriesId = table.Column<int>(type: "integer", nullable: false),
                    SellersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessorySellerXREF", x => new { x.AccessoriesId, x.SellersId });
                    table.ForeignKey(
                        name: "FK_AccessorySellerXREF_Accessories_AccessoriesId",
                        column: x => x.AccessoriesId,
                        principalSchema: "abc",
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessorySellerXREF_Sellers_SellersId",
                        column: x => x.SellersId,
                        principalSchema: "abc",
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessoryId = table.Column<int>(type: "integer", nullable: false),
                    AvailableCount = table.Column<int>(type: "integer", nullable: false),
                    TotalSold = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalSchema: "abc",
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemImages",
                schema: "abc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccessoryId = table.Column<int>(type: "integer", nullable: false),
                    AltText = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemImages_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalSchema: "abc",
                        principalTable: "Accessories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accessories_AccessoryBaseId",
                schema: "abc",
                table: "Accessories",
                column: "AccessoryBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessoryBase_AccessoryBaseId",
                schema: "abc",
                table: "AccessoryBase",
                column: "AccessoryBaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessorySellerXREF_SellersId",
                schema: "abc",
                table: "AccessorySellerXREF",
                column: "SellersId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_AccessoryId",
                schema: "abc",
                table: "Inventory",
                column: "AccessoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemImages_AccessoryId",
                schema: "abc",
                table: "ItemImages",
                column: "AccessoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessorySellerXREF",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "Brands",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "Inventory",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "ItemImages",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "Sellers",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "Accessories",
                schema: "abc");

            migrationBuilder.DropTable(
                name: "AccessoryBase",
                schema: "abc");
        }
    }
}
