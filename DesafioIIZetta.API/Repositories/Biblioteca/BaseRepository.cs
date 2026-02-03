using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories.Biblioteca{
    public class BaseRepository<C> : IBaseRepo<C> where C : class{
        protected readonly ControleEmprestimoLivroContext _context;
            
        public BaseRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }

        public async Task<List<C>> ListarTodosAsync() => await _context.Set<C>().ToListAsync();

        public async Task<C> BuscarPorIdAsync(int id) => await _context.Set<C>().FindAsync(id);

        public async Task AdicionarAsync(C entidade){
            await _context.Set<C>().AddAsync(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(C entidade){
            _context.Set<C>().Update(entidade);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id){
            var entidade = await BuscarPorIdAsync(id);
            if (entidade != null){
                _context.Set<C>().Remove(entidade);
                await _context.SaveChangesAsync();
            }
        }
    }
}
