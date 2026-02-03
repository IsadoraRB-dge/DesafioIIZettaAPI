using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories{
    public class UsuarioRepository : IUsuarioRepository{
        private readonly ControleEmprestimoLivroContext _context;

        public UsuarioRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }

        public async Task<Usuario?> BuscarPorEmailESenhaAsync(string email, string senha){
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.EmailUsuario == email && u.SenhaUsuario == senha);
        }

        public async Task<bool> UsuarioJaExisteAsync(string email){
            return await _context.Usuarios.AnyAsync(u => u.EmailUsuario == email);
        }

        public async Task AdicionarAsync(Usuario usuario){
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> SalvarAlteracoesAsync(){

            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}