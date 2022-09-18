using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.API.Data.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8ef65017-d800-4026-9819-ac06ae871f45", "573c7acd-c0d7-4fa7-80df-fb3174a61363", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c440b82d-fd91-433f-bfe5-65b92787f732", "a0edc3b6-360c-4766-86a1-fabaa67fee6d", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ef65017-d800-4026-9819-ac06ae871f45");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c440b82d-fd91-433f-bfe5-65b92787f732");
        }
    }
}
