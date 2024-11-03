using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DentalManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseStructureAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Dentist_DentistId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Dentist_AspNetUsers_UserId",
                table: "Dentist");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Dentist_DentistId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Dentist_DentistId",
                table: "Procedures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dentist",
                table: "Dentist");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Dentist",
                newName: "Dentists");

            migrationBuilder.RenameIndex(
                name: "IX_Dentist_UserId",
                table: "Dentists",
                newName: "IX_Dentists_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContact",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Allergies",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dentists",
                table: "Dentists",
                column: "DentistId");

            migrationBuilder.InsertData(
                table: "AppointmentsTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Consultation" },
                    { 2, "Examination" },
                    { 3, "Routine care" },
                    { 4, "Surgery" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Dentists_DentistId",
                table: "Appointments",
                column: "DentistId",
                principalTable: "Dentists",
                principalColumn: "DentistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dentists_AspNetUsers_UserId",
                table: "Dentists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Dentists_DentistId",
                table: "Prescriptions",
                column: "DentistId",
                principalTable: "Dentists",
                principalColumn: "DentistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Dentists_DentistId",
                table: "Procedures",
                column: "DentistId",
                principalTable: "Dentists",
                principalColumn: "DentistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Dentists_DentistId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Dentists_AspNetUsers_UserId",
                table: "Dentists");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Dentists_DentistId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Dentists_DentistId",
                table: "Procedures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dentists",
                table: "Dentists");

            migrationBuilder.DeleteData(
                table: "AppointmentsTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppointmentsTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppointmentsTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppointmentsTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameTable(
                name: "Dentists",
                newName: "Dentist");

            migrationBuilder.RenameIndex(
                name: "IX_Dentists_UserId",
                table: "Dentist",
                newName: "IX_Dentist_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmergencyContact",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Allergies",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dentist",
                table: "Dentist",
                column: "DentistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Dentist_DentistId",
                table: "Appointments",
                column: "DentistId",
                principalTable: "Dentist",
                principalColumn: "DentistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dentist_AspNetUsers_UserId",
                table: "Dentist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Dentist_DentistId",
                table: "Prescriptions",
                column: "DentistId",
                principalTable: "Dentist",
                principalColumn: "DentistId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Dentist_DentistId",
                table: "Procedures",
                column: "DentistId",
                principalTable: "Dentist",
                principalColumn: "DentistId");
        }
    }
}
