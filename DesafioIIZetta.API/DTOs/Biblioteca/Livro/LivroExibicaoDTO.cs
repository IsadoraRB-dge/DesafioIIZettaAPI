namespace DesafioIIZetta.API.DTOs.Livro{
    public class LivroExibicaoDTO{
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public int Estoque { get; set; }
    }
}