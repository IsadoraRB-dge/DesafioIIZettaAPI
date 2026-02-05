using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.Biblioteca;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories.Biblioteca{
    public class EmprestimoRepository : IEmprestimoRepo
    {
        private readonly ControleEmprestimoLivroContext _context;

        public EmprestimoRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }

        public async Task<List<ClienteLivroEmprestimo>> ListarTodosAsync(int usuarioId){
            return await _context.ClienteLivroEmprestimos
                .Where(e => e.IdUsuario == usuarioId)
                // É adicionado os  dados do Cliente e do Livro para evitar múltiplas consultas ao banco(N+1)
                .Include(e => e.IdClienteNavigation)
                .Include(e => e.IdLivroNavigation)
                .ToListAsync();
        }

        public async Task RegistrarEmprestimoAsync(ClienteLivroEmprestimo emprestimo){
            var livro = await _context.Livros
                .FirstOrDefaultAsync(l => l.IdLivro == emprestimo.IdLivro && l.IdUsuario == emprestimo.IdUsuario);

            if (livro == null)
                throw new BibliotecaException("O livro selecionado não existe ou não pertence à sua conta.");
                //Validação de estoque antes de efetivar o empréstimo
                if (livro.QuantidadeEstoqueLivro <= 0)
                    throw new BibliotecaException("Este livro não possui exemplares disponíveis no estoque no momento.");
                        //Decrementa o estoque do livro automaticamente
                        livro.QuantidadeEstoqueLivro -= 1;

                        if (emprestimo.DataEmprestimo == DateTime.MinValue)
                            emprestimo.DataEmprestimo = DateTime.Now;

                        await _context.ClienteLivroEmprestimos.AddAsync(emprestimo);
                        // O SaveChanges encapsula ambas as alterações (estoque e novo empréstimo) em uma única transação SQL
                        await _context.SaveChangesAsync();
        }

        public async Task RegistrarDevolucaoAsync(int id, int usuarioId, DateTime? dataManual)
        {
            var emprestimo = await _context.ClienteLivroEmprestimos
                .FirstOrDefaultAsync(e => e.Id == id && e.IdUsuario == usuarioId);

            if (emprestimo == null)
                throw new BibliotecaException("O registro de empréstimo informado não foi localizado.");

            if (emprestimo.DataDevolucaoReal != null)
                throw new BibliotecaException("Este empréstimo já consta como devolvido no sistema.");

            var livro = await _context.Livros
                    .FirstOrDefaultAsync(l => l.IdLivro == emprestimo.IdLivro && l.IdUsuario == usuarioId);
                    // Incrementa o estoque ao devolver o livro
                    if (livro != null){
                        livro.QuantidadeEstoqueLivro += 1;
                    }
                    // Permite retroagir a data de devolução se necessário, ou usa a data atual (Default)
                    emprestimo.DataDevolucaoReal = dataManual ?? DateTime.Now;
                        await _context.SaveChangesAsync();
        }

        public async Task<ClienteLivroEmprestimo> BuscarPorIdAsync(int id, int usuarioId){
            var emprestimo = await _context.ClienteLivroEmprestimos
                .Include(e => e.IdClienteNavigation)
                .Include(e => e.IdLivroNavigation)
                .FirstOrDefaultAsync(e => e.Id == id && e.IdUsuario == usuarioId);

            if (emprestimo == null)
                throw new BibliotecaException("Empréstimo não encontrado.");

            return emprestimo;
        }
        public async Task AtualizarAsync(ClienteLivroEmprestimo emprestimo){
            // O estado é explicitado como Modified para garantir que o EF rastreie as alterações, 
            // útil para objetos que venham de fora do contexto.
            _context.Entry(emprestimo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}