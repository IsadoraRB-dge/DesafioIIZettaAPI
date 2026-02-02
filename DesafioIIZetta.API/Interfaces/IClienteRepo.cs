using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Interfaces {

    public interface IClienteRepo : IBaseRepo<Cliente>{

        new Task<Cliente> BuscarPorIdAsync(int id);
    }
}
