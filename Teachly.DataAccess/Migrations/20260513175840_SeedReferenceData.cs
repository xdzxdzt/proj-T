using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Teachly.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedReferenceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Institutions",
                columns: new[] { "Id", "City", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("0deaa567-50ad-46c5-926f-e8d35190236a"), "Самара", "Самарский колледж сервиса производственного оборудования", "College" },
                    { new Guid("1b2d6d50-c1d6-4f63-8a5a-ffbb7f4f6e5a"), "Самара", "Средняя общеобразовательная школа N 1", "School" },
                    { new Guid("80b2760a-77d4-4bda-a501-9f42688bc8d4"), "Самара", "Лицей информационных технологий", "School" },
                    { new Guid("87090f4c-40c6-4fb5-8e3e-bd7530362f50"), "Самара", "Самарский университет", "University" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("334a4427-66eb-411c-903a-f7c2a62b9f7c"), "Английский язык" },
                    { new Guid("3ba55ab4-9ba4-491e-a3cc-752067db3b91"), "Биология" },
                    { new Guid("4e3359ab-542a-4486-99e1-69cf6651f3a1"), "Математика" },
                    { new Guid("a3901178-731e-4691-80b7-62d322735f18"), "Физика" },
                    { new Guid("b7a24854-3d03-4c31-bc00-7e4ce5d07aaa"), "История" },
                    { new Guid("e7b2ff76-a47d-4da2-8755-ae617de01f94"), "Русский язык" },
                    { new Guid("f27ce41c-d725-417c-b49f-f668918c34db"), "Информатика" },
                    { new Guid("f54acc99-b9b0-4bcf-8d98-8e5842959be3"), "Химия" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: new Guid("0deaa567-50ad-46c5-926f-e8d35190236a"));

            migrationBuilder.DeleteData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: new Guid("1b2d6d50-c1d6-4f63-8a5a-ffbb7f4f6e5a"));

            migrationBuilder.DeleteData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: new Guid("80b2760a-77d4-4bda-a501-9f42688bc8d4"));

            migrationBuilder.DeleteData(
                table: "Institutions",
                keyColumn: "Id",
                keyValue: new Guid("87090f4c-40c6-4fb5-8e3e-bd7530362f50"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("334a4427-66eb-411c-903a-f7c2a62b9f7c"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("3ba55ab4-9ba4-491e-a3cc-752067db3b91"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("4e3359ab-542a-4486-99e1-69cf6651f3a1"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a3901178-731e-4691-80b7-62d322735f18"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("b7a24854-3d03-4c31-bc00-7e4ce5d07aaa"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e7b2ff76-a47d-4da2-8755-ae617de01f94"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f27ce41c-d725-417c-b49f-f668918c34db"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("f54acc99-b9b0-4bcf-8d98-8e5842959be3"));
        }
    }
}
