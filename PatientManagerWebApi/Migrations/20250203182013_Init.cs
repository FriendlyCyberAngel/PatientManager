using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientManagerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pm_patient",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pm_patient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pm_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<string>(type: "text", nullable: true),
                    login = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pm_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pm_recommendation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    patient_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    completed = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pm_recommendation", x => x.id);
                    table.ForeignKey(
                        name: "FK_pm_recommendation_pm_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "pm_patient",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_pm_recommendation_patient_id",
                table: "pm_recommendation",
                column: "patient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pm_recommendation");

            migrationBuilder.DropTable(
                name: "pm_user");

            migrationBuilder.DropTable(
                name: "pm_patient");
        }
    }
}
