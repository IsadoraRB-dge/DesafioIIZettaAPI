using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.Livro{
    public class LivroAtualizarDTO{
        [Required(ErrorMessage = "O ID é obrigatório para atualização.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título do livro é obrigatório.")]
        [StringLength(150, ErrorMessage = "O título não pode exceder 150 caracteres.")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O autor é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome do autor não pode exceder 150 caracteres.")]
        public required string Autor { get; set; }

        [Range(1000, 2100, ErrorMessage = "O ano deve ser válido.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
        public int Estoque { get; set; }
    }
}