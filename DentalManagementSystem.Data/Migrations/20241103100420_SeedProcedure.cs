using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DentalManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("326f6b55-1956-4fde-b170-7ec9ad70fbbc"));

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("6d70d5fd-ef1e-4b20-b278-aaf6512abad0"), new DateTime(2024, 11, 3, 10, 4, 19, 894, DateTimeKind.Utc).AddTicks(708), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });

            migrationBuilder.InsertData(
                table: "Procedures",
                columns: new[] { "ProcedureId", "DentistId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, "Professional cleaning to remove tartar and plaque using an ultrasonic scaler.", "Teeth Cleaning", 30m },
                    { 2, null, "Treatment of dental cavities with high-quality composite filling to restore the tooth's function and aesthetics.", "Cavity Filling", 70m },
                    { 3, null, "Removal of a tooth due to damage, infection, or orthodontic reasons.", "Tooth Extraction", 80m },
                    { 4, null, "Professional whitening procedure using special gels and lamps to achieve a brighter smile.", "Teeth Whitening", 150m },
                    { 5, null, "Placement of thin porcelain veneers to improve the appearance of front teeth, giving them a natural and attractive look", "Veneers", 250m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: new Guid("6d70d5fd-ef1e-4b20-b278-aaf6512abad0"));

            migrationBuilder.DeleteData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Procedures",
                keyColumn: "ProcedureId",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "AppointmentDate", "AppointmentStatus", "AppointmentTypeId", "DentistId", "PatientId" },
                values: new object[] { new Guid("326f6b55-1956-4fde-b170-7ec9ad70fbbc"), new DateTime(2024, 11, 3, 10, 0, 31, 330, DateTimeKind.Utc).AddTicks(3377), 0, 1, new Guid("ffa779be-a8d5-4662-95f2-d5771ae8a22a"), new Guid("d6c3bedb-81e7-4315-8e55-841b06bdd51f") });
        }
    }
}
