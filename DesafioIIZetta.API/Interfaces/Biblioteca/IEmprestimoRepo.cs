using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Interfaces.Biblioteca{
    public interface IEmprestimoRepo{

        Task<List<ClienteLivroEmprestimo>> ListarTodosAsync();

        Task<ClienteLivroEmprestimo> BuscarPorIdAsync(int id);

        Task RegistrarEmprestimoAsync(ClienteLivroEmprestimo emprestimo);

        Task RegistrarDevolucaoAsync(int id);

        Task AtualizarAsync(ClienteLivroEmprestimo emprestimo);
    }
}
