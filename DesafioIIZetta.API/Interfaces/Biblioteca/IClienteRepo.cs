using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models.Biblioteca;

namespace DesafioIIZetta.API.Interfaces{
    public interface IClienteRepo : IBaseRepo<Cliente>{
        Task<Cliente> BuscarPorCpfAsync(string cpf, int usuarioId);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}