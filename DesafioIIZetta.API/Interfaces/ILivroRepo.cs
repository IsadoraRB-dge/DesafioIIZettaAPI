using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Interfaces{

    public interface ILivroRepo {

        Task<List<Livro>> ListarTodosAsync();

        Task<Livro> BuscarPorIdAsync(int id);

        Task AdicionarAsync(Livro livro);

        Task AtualizarAsync(Livro livro);

        Task ExcluirAsync(int id);
    }
}
