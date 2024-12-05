using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDeleteForProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("d15c98cd-d68d-442b-8307-b6d000fc1ab9"));*/

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Procedures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            /*migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("3c9826bc-570d-4845-958d-b4f5090bbeda"), new DateTime(2024, 12, 5, 17, 28, 15, 809, DateTimeKind.Utc).AddTicks(3406), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 1,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 2,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 3,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 4,
                columns: new string[0],
                values: new object[0]);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 5,
                columns: new string[0],
                values: new object[0]);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("3c9826bc-570d-4845-958d-b4f5090bbeda"));*/

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Procedures");

            /*migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("d15c98cd-d68d-442b-8307-b6d000fc1ab9"), new DateTime(2024, 12, 5, 17, 0, 48, 923, DateTimeKind.Utc).AddTicks(9358), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });*/
        }
    }
}
