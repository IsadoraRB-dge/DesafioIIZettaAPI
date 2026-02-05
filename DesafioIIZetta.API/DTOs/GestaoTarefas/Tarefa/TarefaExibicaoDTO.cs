namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa{
    public class TarefaExibicaoDTO{
        public int IdTarefa { get; set; }
        public required string NomeTarefa { get; set; }
        public required string DescricaoTarefa { get; set; }
        public required string StatusTarefa { get; set; }
        public required string Prioridade { get; set; }
    }
}