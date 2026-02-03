using AutoMapper;
using DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarefaExibicaoDTO>>> GetTarefas([FromQuery] string? status, [FromQuery] string? prioridade){
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var tarefas = await _tarefaRepository.ListarTarefasAsync(usuarioId, status, prioridade);

            return Ok(_mapper.Map<IEnumerable<TarefaExibicaoDTO>>(tarefas));
        }

        [HttpPost]
        public async Task<ActionResult> PostTarefa(TarefaAdicionarDTO tarefaDto){
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var tarefa = _mapper.Map<Tarefa>(tarefaDto);
            tarefa.IdUsuario = usuarioId;

            await _tarefaRepository.AdicionarAsync(tarefa);
            await _tarefaRepository.SalvarAlteracoesAsync();

            return Ok(new { mensagem = "Tarefa criada com sucesso!", id = tarefa.IdTarefa });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarefa(int id, TarefaAtualizarDTO tarefaDto){
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var tarefaBanco = await _tarefaRepository.BuscarPorIdAsync(id, usuarioId);

            if (tarefaBanco == null)
                return NotFound("Tarefa não encontrada ou acesso negado.");
                _mapper.Map(tarefaDto, tarefaBanco);

            await _tarefaRepository.AtualizarAsync(tarefaBanco);
            await _tarefaRepository.SalvarAlteracoesAsync();

            return Ok("Tarefa atualizada com sucesso!");
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarefa(int id){
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var tarefa = await _tarefaRepository.BuscarPorIdAsync(id, usuarioId);

            if (tarefa == null)
                return NotFound("Tarefa não encontrada.");

            await _tarefaRepository.DeletarAsync(tarefa);
            await _tarefaRepository.SalvarAlteracoesAsync();

            return Ok("Tarefa removida!");
        }
    }
}