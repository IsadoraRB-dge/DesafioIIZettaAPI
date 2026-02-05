using AutoMapper;
using DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa;
using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces; 
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DesafioIIZetta.API.Controllers.GestaoTarefas{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase{
        private readonly ITarefaRepository _tarefaRepository; 
        private readonly IMapper _mapper;

        public TarefasController(ITarefaRepository tarefaRepository, IMapper mapper){
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
        }

        private int ObterUsuarioLogadoId(){
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaExibicaoDTO>>> GetTarefas([FromQuery] string? status, [FromQuery] string? prioridade){
            var usuarioId = ObterUsuarioLogadoId();
            //Permite que o usuário refine a busca por status ou prioridade, mantendo a segurança de exibir apenas suas próprias tarefas.
            var tarefas = await _tarefaRepository.ListarTarefasAsync(usuarioId, status, prioridade);
            return Ok(_mapper.Map<IEnumerable<TarefaExibicaoDTO>>(tarefas));
        }

        [HttpPost]
        public async Task<ActionResult> PostTarefa(TarefaAdicionarDTO tarefaDto){

            var tarefa = _mapper.Map<Tarefa>(tarefaDto);
            // Define o dono da tarefa com base no token JWT antes de persistir.
            tarefa.IdUsuario = ObterUsuarioLogadoId();

            await _tarefaRepository.AdicionarAsync(tarefa);
            await _tarefaRepository.SalvarAlteracoesAsync();

            return Ok(new { mensagem = "Tarefa criada com sucesso!", id = tarefa.IdTarefa });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, TarefaAtualizarDTO tarefaDto) {
            var usuarioId = ObterUsuarioLogadoId();
            // Busca a tarefa existente para garantir que ela pertença ao usuário logado antes de atualizar.
            var tarefaBanco = await _tarefaRepository.BuscarPorIdAsync(id, usuarioId);

            if (tarefaBanco == null)
                throw new TarefaException("Tarefa não encontrada.");

            _mapper.Map(tarefaDto, tarefaBanco);

            await _tarefaRepository.AtualizarAsync(tarefaBanco);
            await _tarefaRepository.SalvarAlteracoesAsync();

            return Ok(new { mensagem = "Tarefa atualizada com sucesso!" });
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id){
            var usuarioId = ObterUsuarioLogadoId();

            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id, usuarioId);
            // A exclusão é feita de forma segura, validando a existência e a posse do recurso.
            await _tarefaRepository.DeletarAsync(tarefa);
            await _tarefaRepository.SalvarAlteracoesAsync();

            return Ok(new { mensagem = "Tarefa removida com sucesso!" });
        }
    }
}