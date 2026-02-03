using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories{
    public class TarefaRepository : ITarefaRepository{
        private readonly ControleEmprestimoLivroContext _context;

        public TarefaRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> ListarTarefasAsync(int usuarioId, string? status, string? prioridade){
            var query = _context.Tarefas.Where(t => t.IdUsuario == usuarioId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.StatusTarefa.ToLower() == status.ToLower());

            if (!string.IsNullOrEmpty(prioridade))
                query = query.Where(t => t.Prioridade.ToLower() == prioridade.ToLower());

            return await query.ToListAsync();
        }

        public async Task<Tarefa?> BuscarPorIdAsync(int id, int usuarioId){
            return await _context.Tarefas
                .FirstOrDefaultAsync(t => t.IdTarefa == id && t.IdUsuario == usuarioId);
        }

        public async Task AdicionarAsync(Tarefa tarefa){
            await _context.Tarefas.AddAsync(tarefa);
        }

        public async Task AtualizarAsync(Tarefa tarefa){
            _context.Tarefas.Update(tarefa);
            await Task.CompletedTask;
        }

        public async Task DeletarAsync(Tarefa tarefa){
            _context.Tarefas.Remove(tarefa);
            await Task.CompletedTask;
        }

        public async Task<bool> SalvarAlteracoesAsync(){

            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}