using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infinite.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewEnumsInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "CapitalSourceType",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Complexity",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "ElaborationObjectives",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Objective",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapitalSourceType",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Complexity",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ElaborationObjectives",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Objective",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Projects");
        }
    }
}
