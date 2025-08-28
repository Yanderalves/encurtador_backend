using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncurtadorURL.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_URLs",
                table: "URLs");

            migrationBuilder.RenameColumn(
                name: "ChaveEncurtadora",
                table: "URLs",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_URLs",
                table: "URLs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_URLs",
                table: "URLs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "URLs",
                newName: "ChaveEncurtadora");

            migrationBuilder.AddPrimaryKey(
                name: "PK_URLs",
                table: "URLs",
                column: "URLEncurtada");
        }
    }
}
