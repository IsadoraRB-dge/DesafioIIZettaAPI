using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Interfaces.Biblioteca{
    public interface IEmprestimoRepo{
        Task<List<ClienteLivroEmprestimo>> ListarTodosAsync(int usuarioId);
        Task<ClienteLivroEmprestimo> BuscarPorIdAsync(int id, int usuarioId);
        Task RegistrarEmprestimoAsync(ClienteLivroEmprestimo emprestimo);
        Task RegistrarDevolucaoAsync(int id, int usuarioId, DateTime? dataManual);
        Task AtualizarAsync(ClienteLivroEmprestimo emprestimo);
    }
}