using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Migrations
{
    /// <inheritdoc />
    public partial class ExclusaoCampoNomeTabelaPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Pedido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
