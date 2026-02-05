using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Excecoes;

namespace DesafioIIZetta.API.Repositories{
    public class TarefaRepository : ITarefaRepository{
        private readonly ControleEmprestimoLivroContext _context;

        public TarefaRepository(ControleEmprestimoLivroContext context){
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> ListarTarefasAsync(int usuarioId, string? status, string? prioridade){
            // A query é iniciada filtrando pelo dono da tarefa 
            var query = _context.Tarefas.Where(t => t.IdUsuario == usuarioId);
            // O SQL final só será executado no banco ao chamar o ToListAsync
            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.StatusTarefa.ToLower() == status.ToLower());

                if (!string.IsNullOrEmpty(prioridade))
                    query = query.Where(t => t.Prioridade.ToLower() == prioridade.ToLower());

            return await query.ToListAsync();
        }

        public async Task<Tarefa> BuscarPorIdAsync(int id, int usuarioId){
            var tarefa = await _context.Tarefas
                .FirstOrDefaultAsync(t => t.IdTarefa == id && t.IdUsuario == usuarioId);

            if (tarefa == null)
                throw new TarefaException("A tarefa solicitada não foi encontrada ou você não tem permissão para acessá-la.");

            return tarefa;
        }

        public async Task AdicionarAsync(Tarefa tarefa){
            await _context.Tarefas.AddAsync(tarefa);
        }

        public async Task AtualizarAsync(Tarefa tarefa){
            await Task.CompletedTask;
        }

        public async Task DeletarAsync(Tarefa tarefa){
            _context.Tarefas.Remove(tarefa);
            await Task.CompletedTask;
        }

        public async Task<bool> SalvarAlteracoesAsync(){
            // A persistência é centralizada para garantir que múltiplas operações 
            // possam ser salvas em uma única transação atômica.
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}