namespace DesafioIIZetta.API.Interfaces{
    public interface IBaseRepo<C> where C : class{
        Task<List<C>> ListarTodosAsync();

        Task<C> BuscarPorIdAsync(int id);

        Task AdicionarAsync(C entidade);

        Task AtualizarAsync(C entidade);

        Task ExcluirAsync(int id);
    }
}
