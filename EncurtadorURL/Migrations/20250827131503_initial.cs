using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncurtadorURL.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "URLs",
                columns: table => new
                {
                    URLEncurtada = table.Column<string>(type: "TEXT", nullable: false),
                    URLOriginal = table.Column<string>(type: "TEXT", nullable: false),
                    ChaveEncurtadora = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_URLs", x => x.URLEncurtada);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "URLs");
        }
    }
}
