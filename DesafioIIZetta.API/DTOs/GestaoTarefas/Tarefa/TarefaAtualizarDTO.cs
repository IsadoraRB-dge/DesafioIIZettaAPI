namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa{
    public class TarefaAtualizarDTO{
        public int IdTarefa { get; set; } 
        public string NomeTarefa { get; set; }
        public string DescricaoTarefa { get; set; }
        public string StatusTarefa { get; set; }
        public string Prioridade { get; set; }
    }
}