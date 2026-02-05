namespace DesafioIIZetta.API.Interfaces.Biblioteca{
    public interface IBaseRepo<C> where C : class{
        Task<List<C>> ListarTodosAsync(int usuarioId);
        Task<C> BuscarPorIdAsync(int id, int usuarioId);
        Task AdicionarAsync(C entidade);
        Task AtualizarAsync(C entidade);
        Task ExcluirAsync(C entidade);
    }
}
