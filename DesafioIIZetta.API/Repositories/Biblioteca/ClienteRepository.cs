using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.Biblioteca;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories.Biblioteca{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepo{
        public ClienteRepository(ControleEmprestimoLivroContext context) : base(context){  
        }

        public new async Task<Cliente> BuscarPorIdAsync(int id){
            return await _context.Clientes
                .Include(c => c.ClienteLivroEmprestimos)
                .ThenInclude(e => e.IdLivroNavigation)
                .FirstOrDefaultAsync(c => c.IdCliente == id);
        }
    }
}