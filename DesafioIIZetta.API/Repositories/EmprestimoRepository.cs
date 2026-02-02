using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories{
    public class EmprestimoRepository : IEmprestimoRepo{

        private readonly ControleEmprestimoLivroContext _context;

        public EmprestimoRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }
        public async Task<List<ClienteLivroEmprestimo>> ListarTodosAsync(){
            return await _context.ClienteLivroEmprestimos
                .Include(e => e.IdClienteNavigation)
                .Include(e => e.IdLivroNavigation)
                .ToListAsync();
        }
        public async Task RegistrarEmprestimoAsync(ClienteLivroEmprestimo emprestimo){
            var livro = await _context.Livros.FindAsync(emprestimo.IdLivro);

            if (livro != null && livro.QuantidadeEstoqueLivro > 0){
                livro.QuantidadeEstoqueLivro -= 1;
                if (emprestimo.DataEmprestimo == DateTime.MinValue) 
                    emprestimo.DataEmprestimo = DateTime.Now;

                await _context.ClienteLivroEmprestimos.AddAsync(emprestimo);
                await _context.SaveChangesAsync();
            }else{
                throw new Exception("Livro indisponível no estoque!");
            }
        }
        public async Task RegistrarDevolucaoAsync(int id){
            var emprestimo = await _context.ClienteLivroEmprestimos.FindAsync(id);

        
            if (emprestimo != null && emprestimo.DataDevolucaoReal == null){
                var livro = await _context.Livros.FindAsync(emprestimo.IdLivro);
                if (livro != null){
                    livro.QuantidadeEstoqueLivro += 1; 
                }

                emprestimo.DataDevolucaoReal = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<ClienteLivroEmprestimo> BuscarPorIdAsync(int id){
            return await _context.ClienteLivroEmprestimos
                .Include(e => e.IdClienteNavigation)
                .Include(e => e.IdLivroNavigation)
                .FirstOrDefaultAsync(e => e.Id == id); 
        }
        public async Task AtualizarAsync(ClienteLivroEmprestimo emprestimo){
            _context.Entry(emprestimo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
