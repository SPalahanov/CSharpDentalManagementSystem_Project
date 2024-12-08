using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropColumnDentistIdInProcedured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Dentists_DentistId",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_DentistId",
                table: "Procedures");

            /*migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("d74b97db-a784-4fe4-8576-3b0108d79fe4"));*/

            migrationBuilder.DropColumn(
                name: "DentistId",
                table: "Procedures");

            /*migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("c27ca465-fcdc-474e-9dde-35bef1c8a314"), new DateTime(2024, 12, 8, 10, 21, 2, 967, DateTimeKind.Utc).AddTicks(6624), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("c27ca465-fcdc-474e-9dde-35bef1c8a314"));*/

            migrationBuilder.AddColumn<Guid>(
                name: "DentistId",
                table: "Procedures",
                type: "uniqueidentifier",
                nullable: true);

            /*migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("d74b97db-a784-4fe4-8576-3b0108d79fe4"), new DateTime(2024, 12, 5, 18, 32, 9, 101, DateTimeKind.Utc).AddTicks(5765), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });*/

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 1,
                column: "DentistId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 2,
                column: "DentistId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 3,
                column: "DentistId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 4,
                column: "DentistId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 5,
                column: "DentistId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_DentistId",
                table: "Procedures",
                column: "DentistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Dentists_DentistId",
                table: "Procedures",
                column: "DentistId",
                principalTable: "Dentists",
                principalColumn: "DentistId");
        }
    }
}
