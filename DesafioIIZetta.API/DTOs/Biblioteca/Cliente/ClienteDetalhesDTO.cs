namespace DesafioIIZetta.API.DTOs.Biblioteca.Cliente{
    public class ClienteDetalhesDTO{
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Cpf { get; set; }
        public required string Telefone { get; set; }
        public required string Endereco { get; set; }
    }
}