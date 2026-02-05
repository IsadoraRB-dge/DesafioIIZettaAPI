using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa{
    public class TarefaAtualizarDTO
    {
        [Required(ErrorMessage = "O ID da tarefa é obrigatório para a atualização.")]
        public int IdTarefa { get; set; }

        [Required(ErrorMessage = "O nome da tarefa não pode ser removido.")]
        [StringLength(150)]
        public required string NomeTarefa { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500)]
        public required string DescricaoTarefa { get; set; }

        [Required(ErrorMessage = "O status (Pendente,Concluida) deve ser informado.")]
        public required string StatusTarefa { get; set; }

        [Required(ErrorMessage = "A prioridade (Baixa, Média, Alta) é obrigatória.")]
        public required  string Prioridade { get; set; }
    }
}