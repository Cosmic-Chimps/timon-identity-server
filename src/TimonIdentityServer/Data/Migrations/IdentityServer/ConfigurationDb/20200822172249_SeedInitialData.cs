using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimonIdentityServer.Data.Migrations.IdentityServer.ConfigurationDb
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 49, 134, DateTimeKind.Utc).AddTicks(6950));

            migrationBuilder.UpdateData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 49, 137, DateTimeKind.Utc).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 49, 136, DateTimeKind.Utc).AddTicks(6890));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 49, 136, DateTimeKind.Utc).AddTicks(4660));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 49, 136, DateTimeKind.Utc).AddTicks(5440));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApiResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 23, 290, DateTimeKind.Utc).AddTicks(5570));

            migrationBuilder.UpdateData(
                table: "ClientSecrets",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 23, 293, DateTimeKind.Utc).AddTicks(3230));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 23, 292, DateTimeKind.Utc).AddTicks(7970));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 23, 292, DateTimeKind.Utc).AddTicks(5770));

            migrationBuilder.UpdateData(
                table: "IdentityResources",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 8, 22, 17, 22, 23, 292, DateTimeKind.Utc).AddTicks(6550));
        }
    }
}
