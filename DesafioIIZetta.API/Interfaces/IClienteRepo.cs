using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Interfaces {

    public interface IClienteRepo {

        Task<List<Cliente>> ListarTodosAsync();

        Task<Cliente> BuscarPorIdAsync(int id);

        Task AdicionarAsync(Cliente cliente);

        Task AtualizarAsync(Cliente cliente);

        Task ExcluirAsync(int id);
    }
}
