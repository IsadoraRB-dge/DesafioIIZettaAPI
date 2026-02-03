namespace DesafioIIZetta.API.DTOs.Biblioteca.Emprestimo{
    public class EmprestimoAdicionarDTO{
        public int IdCliente { get; set; }
        public int IdLivro { get; set; }
        public int DiasEmprestimo { get; set; }
    }
}