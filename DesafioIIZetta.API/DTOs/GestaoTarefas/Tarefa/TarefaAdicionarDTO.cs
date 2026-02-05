using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa{
    public class TarefaAdicionarDTO{
        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome da tarefa não pode exceder 150 caracteres.")]
        public required string NomeTarefa { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500)]
        public required string DescricaoTarefa { get; set; }

        [Required(ErrorMessage = "O status (Pendente,Concluida) deve ser informado.")]
        public required string StatusTarefa { get; set; } = "Pendente";

        [Required(ErrorMessage = "A prioridade (Baixa, Média, Alta) é obrigatória.")]
        public required string Prioridade { get; set; }
    }
}
