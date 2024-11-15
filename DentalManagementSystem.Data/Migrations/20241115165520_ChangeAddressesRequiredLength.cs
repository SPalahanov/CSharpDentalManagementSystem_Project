using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAddressesRequiredLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("6d70d5fd-ef1e-4b20-b278-aaf6512abad0"));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Patients",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Dentists",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("d43c1c9b-da63-430d-a2f4-a79fac61e87c"), new DateTime(2024, 11, 15, 16, 55, 19, 80, DateTimeKind.Utc).AddTicks(8618), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("d43c1c9b-da63-430d-a2f4-a79fac61e87c"));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Patients",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Dentists",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(70)",
                oldMaxLength: 70);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("6d70d5fd-ef1e-4b20-b278-aaf6512abad0"), new DateTime(2024, 11, 3, 10, 4, 19, 894, DateTimeKind.Utc).AddTicks(708), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });
        }
    }
}
