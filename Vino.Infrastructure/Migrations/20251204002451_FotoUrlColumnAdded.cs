using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterThanYou.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FotoUrlColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Clients",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Clients");
        }
    }
}
