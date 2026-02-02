using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;

namespace DesafioIIZetta.API.Repositories{
    public class LivroRepository : BaseRepository<Livro>, ILivroRepo{
        public LivroRepository(ControleEmprestimoLivroContext context) : base(context){
        }
    }
}