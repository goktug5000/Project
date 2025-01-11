using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class add_capsuleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapsuleData",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.AddColumn<Guid>(
                name: "CapsuleDataId",
                schema: "Project",
                table: "Capsules",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                schema: "Project",
                table: "Capsules",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "Project",
                table: "Capsules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CapsuleData",
                schema: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CapsuleText = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapsuleData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capsules_CapsuleDataId",
                schema: "Project",
                table: "Capsules",
                column: "CapsuleDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Capsules_CapsuleData_CapsuleDataId",
                schema: "Project",
                table: "Capsules",
                column: "CapsuleDataId",
                principalSchema: "Project",
                principalTable: "CapsuleData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capsules_CapsuleData_CapsuleDataId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropTable(
                name: "CapsuleData",
                schema: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Capsules_CapsuleDataId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropColumn(
                name: "CapsuleDataId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropColumn(
                name: "LockDate",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.AddColumn<string>(
                name: "CapsuleData",
                schema: "Project",
                table: "Capsules",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                schema: "Project",
                table: "Capsules",
                type: "bytea",
                nullable: true);
        }
    }
}
