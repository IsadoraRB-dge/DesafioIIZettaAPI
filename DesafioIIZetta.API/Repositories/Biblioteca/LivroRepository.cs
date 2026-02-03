using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Repositories.Biblioteca{
    public class LivroRepository : BaseRepository<Livro>, ILivroRepo{
        public LivroRepository(ControleEmprestimoLivroContext context) : base(context){
        }
    }
}