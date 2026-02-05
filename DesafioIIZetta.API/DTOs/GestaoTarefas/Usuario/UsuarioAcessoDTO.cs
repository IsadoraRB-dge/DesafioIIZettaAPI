using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Usuario{
    public class UsuarioAcessoDTO{
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public required string EmailUsuario { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string SenhaUsuario { get; set; }
    }
}