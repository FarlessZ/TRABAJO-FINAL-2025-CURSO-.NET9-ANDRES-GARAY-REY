using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PortalGalaxy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Estado", "FechaCreacion", "Nombre" },
                values: new object[,]
                {
                    { 2, true, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Java" },
                    { 3, true, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python" },
                    { 4, true, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
