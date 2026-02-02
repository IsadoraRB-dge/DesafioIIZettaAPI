using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories{
    public class LivroRepository : ILivroRepo{

        private readonly ControleEmprestimoLivroContext _context;

        public LivroRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }

        public async Task<List<Livro>> ListarTodosAsync(){
            return await _context.Livros.ToListAsync();
        }

        public async Task<Livro> BuscarPorIdAsync(int id){
            return await _context.Livros.FindAsync(id);
        }

        public async Task AdicionarAsync(Livro livro){
            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Livro livro){
            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id){
            var livro = await BuscarPorIdAsync(id);
            if (livro != null){
                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();
            }
        }
    }
}
