using DesafioIIZetta.API.Models.GestaoTarefas;

namespace DesafioIIZetta.API.Interfaces{
    public interface ITarefaRepository{
        Task<IEnumerable<Tarefa>> ListarTarefasAsync(int usuarioId, string? status, string? prioridade);

        Task<Tarefa> BuscarPorIdAsync(int id, int usuarioId);

        Task AdicionarAsync(Tarefa tarefa);

        Task AtualizarAsync(Tarefa tarefa);

        Task DeletarAsync(Tarefa tarefa);

        Task<bool> SalvarAlteracoesAsync();
    }
}