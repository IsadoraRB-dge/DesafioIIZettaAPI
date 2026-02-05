using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Excecoes;

namespace DesafioIIZetta.API.Repositories{
    public class UsuarioRepository : IUsuarioRepository{
        private readonly ControleEmprestimoLivroContext _context;

        public UsuarioRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }
       

        public async Task<Usuario?> BuscarPorEmailAsync(string email){
            // Busca o usuário de forma case-insensitive para evitar erros comuns no login.
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.EmailUsuario.ToLower() == email.ToLower());
        }

        public async Task<bool> UsuarioJaExisteAsync(string email){
            //Impede a criação de contas duplicadas com o mesmo e-mail.
            var existe = await _context.Usuarios.AnyAsync(u => u.EmailUsuario.ToLower() == email.ToLower());

            if (existe)
                throw new TarefaException("Este e-mail já está cadastrado no sistema.");

            return false;
        }

        public async Task AdicionarAsync(Usuario usuario){
            // Adiciona a entidade ao contexto; a persistência real ocorre no SaveChangesAsync.
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task<bool> SalvarAlteracoesAsync(){
            // Persiste todas as mudanças pendentes no banco de dados.
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}