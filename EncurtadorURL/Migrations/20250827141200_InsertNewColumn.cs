using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncurtadorURL.Migrations
{
    /// <inheritdoc />
    public partial class InsertNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChaveEncurtada",
                table: "URLs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChaveEncurtada",
                table: "URLs");
        }
    }
}
