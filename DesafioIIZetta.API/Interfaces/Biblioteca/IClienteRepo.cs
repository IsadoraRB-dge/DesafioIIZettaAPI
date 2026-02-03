using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models.Biblioteca;

namespace DesafioIIZetta.API.Interfaces {

    public interface IClienteRepo : IBaseRepo<Cliente>{

        new Task<Cliente> BuscarPorIdAsync(int id);
    }
}
