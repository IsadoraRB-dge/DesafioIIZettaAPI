namespace DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa{
    public class TarefaAdicionarDTO{
        public string NomeTarefa { get; set; }
        public string DescricaoTarefa { get; set; }
        public string StatusTarefa { get; set; } = "Pendente";
        public string Prioridade { get; set; }
    }
}
