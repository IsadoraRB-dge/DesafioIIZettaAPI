using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.Biblioteca.Emprestimo{
    public class EmprestimoAdicionarDTO{
        [Required(ErrorMessage = "É necessário informar o cliente.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "É necessário informar o livro.")]
        public int IdLivro { get; set; }

        [Required(ErrorMessage = "Informe o prazo do empréstimo em dias.")]
        [Range(1, 90, ErrorMessage = "O prazo deve ser de no mínimo 1 e no máximo 90 dias.")]
        public int DiasEmprestimo { get; set; }
    }
}