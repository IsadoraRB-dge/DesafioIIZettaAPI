using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories.Biblioteca{
    public class LivroRepository : BaseRepository<Livro>, ILivroRepo
    {
        public LivroRepository(ControleEmprestimoLivroContext context) : base(context)  { }

        // Sobrescreve a busca por ID para incluir o histórico de circulação do livro
        public override async Task<Livro> BuscarPorIdAsync(int id, int usuarioId)
        {
            // O livro é carregado junto com seus empréstimos e os nomes dos respectivos clientes.
            // Isso permite que o bibliotecário veja quem está com o exemplar.
            var livro = await _context.Livros
                .Include(l => l.ClienteLivroEmprestimos) 
                    .ThenInclude(e => e.IdClienteNavigation)
                .FirstOrDefaultAsync(l => l.IdLivro == id && l.IdUsuario == usuarioId);

            if (livro == null)
                throw new BibliotecaException("Livro não encontrado ou você não possui permissão para acessá-lo.");

            return livro;
        }
    }
}