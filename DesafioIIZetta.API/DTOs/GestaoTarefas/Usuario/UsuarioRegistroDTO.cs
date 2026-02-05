using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Usuario{
    public class UsuarioRegistroDTO{
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public required string NomeUsuario { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail é inválido.")]
        public required string EmailUsuario { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string SenhaUsuario { get; set; }
    }
}
