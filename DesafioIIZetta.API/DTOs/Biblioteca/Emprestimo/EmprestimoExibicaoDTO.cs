namespace DesafioIIZetta.API.DTOs.Biblioteca.Emprestimo{
    public class EmprestimoExibicaoDTO{
        public int Id { get; set; }
        public required string NomeCliente { get; set; }
        public required string TituloLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public decimal ValorMulta { get; set; }
        public int DiasAtraso { get; set; }
        public bool EstaAtrasado { get; set; }
    }
}