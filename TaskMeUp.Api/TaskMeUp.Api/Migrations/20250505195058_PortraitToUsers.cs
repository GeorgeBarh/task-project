using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMeUp.Api.Migrations
{
    /// <inheritdoc />
    public partial class PortraitToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Portrait",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Portrait",
                table: "Users");
        }
    }
}
