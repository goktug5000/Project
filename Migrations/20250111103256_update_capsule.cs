using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class update_capsule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capsules_AspNetUsers_CreaterIdId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropIndex(
                name: "IX_Capsules_CreaterIdId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.DropColumn(
                name: "CreaterIdId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.AddColumn<Guid>(
                name: "CreaterId",
                schema: "Project",
                table: "Capsules",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreaterId",
                schema: "Project",
                table: "Capsules");

            migrationBuilder.AddColumn<string>(
                name: "CreaterIdId",
                schema: "Project",
                table: "Capsules",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Capsules_CreaterIdId",
                schema: "Project",
                table: "Capsules",
                column: "CreaterIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Capsules_AspNetUsers_CreaterIdId",
                schema: "Project",
                table: "Capsules",
                column: "CreaterIdId",
                principalSchema: "Project",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
