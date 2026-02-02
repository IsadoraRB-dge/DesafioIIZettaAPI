using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories{
    public class ClienteRepository : IClienteRepo{

        private readonly ControleEmprestimoLivroContext _context;

        public ClienteRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }
        public async Task<List<Cliente>> ListarTodosAsync(){
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> BuscarPorIdAsync(int id){
            return await _context.Clientes.FindAsync(id);
        }

        public async Task AdicionarAsync(Cliente cliente){
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente cliente){
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }
        public async Task ExcluirAsync(int id){
            var cliente = await BuscarPorIdAsync(id);
            if (cliente != null){
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
