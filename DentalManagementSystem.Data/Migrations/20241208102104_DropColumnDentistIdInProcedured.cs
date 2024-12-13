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

            migrationBuilder.DropColumn(
                name: "DentistId",
                table: "Procedures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DentistId",
                table: "Procedures",
                type: "uniqueidentifier",
                nullable: true);

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
