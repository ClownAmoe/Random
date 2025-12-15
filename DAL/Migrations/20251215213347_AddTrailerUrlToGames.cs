using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOverDose.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTrailerUrlToGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "trailer_url",
                table: "games",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "trailer_url",
                table: "games");
        }
    }
}
