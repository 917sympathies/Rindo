using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rindo.API.Migrations
{
    /// <inheritdoc />
    public partial class tagsRemoveColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tags",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
