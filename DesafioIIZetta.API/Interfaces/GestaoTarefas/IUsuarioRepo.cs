using DesafioIIZetta.API.Models.GestaoTarefas;

namespace DesafioIIZetta.API.Interfaces
{
    public interface IUsuarioRepository{
       
        Task<Usuario?> BuscarPorEmailAsync(string email);

        Task<bool> UsuarioJaExisteAsync(string email);
        Task AdicionarAsync(Usuario usuario);

        Task<bool> SalvarAlteracoesAsync();
    }
}