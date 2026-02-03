namespace DesafioIIZetta.API.DTOs.Emprestimo{
    public class EmprestimoExibicaoDTO{
        public int Id { get; set; }
        public string NomeCliente { get; set; }
        public string TituloLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
    }
}