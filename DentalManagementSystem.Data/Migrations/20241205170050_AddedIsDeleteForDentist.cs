using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDeleteForDentist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("f32fa48f-86eb-44ec-81a4-9d812efee86f"));*/

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Dentists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            /*migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("d15c98cd-d68d-442b-8307-b6d000fc1ab9"), new DateTime(2024, 12, 5, 17, 0, 48, 923, DateTimeKind.Utc).AddTicks(9358), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("d15c98cd-d68d-442b-8307-b6d000fc1ab9"));*/

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Dentists");

            /*migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("f32fa48f-86eb-44ec-81a4-9d812efee86f"), new DateTime(2024, 12, 5, 2, 49, 9, 678, DateTimeKind.Utc).AddTicks(8010), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });*/
        }
    }
}
