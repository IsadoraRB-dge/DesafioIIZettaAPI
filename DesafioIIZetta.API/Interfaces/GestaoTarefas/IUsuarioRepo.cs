using DesafioIIZetta.API.Models.GestaoTarefas;

namespace DesafioIIZetta.API.Interfaces{
    public interface IUsuarioRepository{
        Task<Usuario?> BuscarPorEmailESenhaAsync(string email, string senha);

        Task<bool> UsuarioJaExisteAsync(string email);

        Task AdicionarAsync(Usuario usuario);

        Task<bool> SalvarAlteracoesAsync();
    }
}