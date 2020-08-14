using Microsoft.EntityFrameworkCore.Migrations;

namespace TimonIdentityServer.Data.Migrations.AspNetIdentity.AspNetIdentityDb
{
    public partial class FirstNameLastNameApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                2);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                3);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                4);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                5);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                6);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                7);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                8);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                9);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                10);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                11);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                12);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                13);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                14);

            migrationBuilder.DeleteData(
                "AspNetUserClaims",
                "Id",
                15);

            migrationBuilder.DeleteData(
                "AspNetUsers",
                "Id",
                "1");

            migrationBuilder.DeleteData(
                "AspNetUsers",
                "Id",
                "2");

            migrationBuilder.AddColumn<string>(
                "FirstName",
                "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "LastName",
                "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "FirstName",
                "AspNetUsers");

            migrationBuilder.DropColumn(
                "LastName",
                "AspNetUsers");

            migrationBuilder.InsertData(
                "AspNetUsers",
                new[]
                {
                    "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled",
                    "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber",
                    "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName"
                },
                new object[,]
                {
                    {
                        "1", 0, "99f49c70-48c3-4d42-9af9-e08fde940e1c", "AliceSmith@email.com", true, false, null,
                        "ALICESMITH@EMAIL.COM", "ALICE",
                        "AQAAAAEAACcQAAAAECPbw8cP+0QSC96lHylWyra5tjqPY9NYo//JWYDKupAuYKq7gLTsbc8+FoG/2p7T0Q==", null,
                        false, "1ef58913-00f0-4c42-8bed-b7e6b95ad2f1", false, "alice"
                    },
                    {
                        "2", 0, "d81297d5-1abc-4baf-9441-bca0bff00574", "BobSmith@email.com", true, false, null,
                        "BOBSMITH@EMAIL.COM", "BOB",
                        "AQAAAAEAACcQAAAAECXOeYB+hSXH72IXO+lF+yCTTmT2qvgyA6hLTNM8iPXm2qMSYt7gpxrAV61i3QDCow==", null,
                        false, "fd3eaa89-11b4-406d-bad9-f0021a79ce0a", false, "bob"
                    }
                });

            migrationBuilder.InsertData(
                "AspNetUserClaims",
                new[] {"Id", "ClaimType", "ClaimValue", "UserId"},
                new object[,]
                {
                    {1, "name", "Alice Smith", "1"},
                    {2, "given_name", "Alice", "1"},
                    {3, "family_name", "Smith", "1"},
                    {4, "email", "AliceSmith@email.com", "1"},
                    {5, "website", "http://alice.com", "1"},
                    {11, "email_verified", "True", "1"},
                    {
                        13, "address",
                        "{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        "1"
                    },
                    {15, "location", "somewhere", "1"},
                    {6, "name", "Bob Smith", "2"},
                    {7, "given_name", "Bob", "2"},
                    {8, "family_name", "Smith", "2"},
                    {9, "email", "BobSmith@email.com", "2"},
                    {10, "website", "http://bob.com", "2"},
                    {12, "email_verified", "True", "2"},
                    {
                        14, "address",
                        "{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        "2"
                    }
                });
        }
    }
}