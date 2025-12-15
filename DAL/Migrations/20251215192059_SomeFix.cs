using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOverDose.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SomeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description_text",
                table: "games",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description_text",
                table: "games");
        }
    }
}
