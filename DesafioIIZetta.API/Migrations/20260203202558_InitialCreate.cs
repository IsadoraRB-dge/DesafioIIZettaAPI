using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioIIZetta.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCliente = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CPFCliente = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    TelefoneCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmailCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EnderecoCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    IdLivro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TituloLivro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AutorLivro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AnoPublicacaoLivro = table.Column<int>(type: "int", nullable: false),
                    QuantidadeEstoqueLivro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.IdLivro);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUsuario = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EmailUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenhaUsuario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Cliente_Livro_Emprestimo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdLivro = table.Column<int>(type: "int", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataDevolucaoPrevista = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataDevolucaoReal = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente_Livro_Emprestimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cliente_Livro_Emprestimo_Cliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK_Cliente_Livro_Emprestimo_Livro",
                        column: x => x.IdLivro,
                        principalTable: "Livro",
                        principalColumn: "IdLivro");
                });

            migrationBuilder.CreateTable(
                name: "Tarefa",
                columns: table => new
                {
                    IdTarefa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTarefa = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DescricaoTarefa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StatusTarefa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Prioridade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefa", x => x.IdTarefa);
                    table.ForeignKey(
                        name: "FK_Tarefa_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Livro_Emprestimo_IdCliente",
                table: "Cliente_Livro_Emprestimo",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Livro_Emprestimo_IdLivro",
                table: "Cliente_Livro_Emprestimo",
                column: "IdLivro");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefa_IdUsuario",
                table: "Tarefa",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cliente_Livro_Emprestimo");

            migrationBuilder.DropTable(
                name: "Tarefa");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Livro");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
