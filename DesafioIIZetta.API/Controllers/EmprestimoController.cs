using AutoMapper;
using DesafioIIZetta.API.DTOs.Emprestimo;
using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIIZetta.API.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase{
        private readonly IEmprestimoRepo _emprestimoRepo;
        private readonly IMapper _mapper;

        public EmprestimoController(IEmprestimoRepo emprestimoRepo, IMapper mapper)
        {
            _emprestimoRepo = emprestimoRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmprestimoExibicaoDTO>>> GetEmprestimo(){
            var lista = await _emprestimoRepo.ListarTodosAsync();
            return Ok(_mapper.Map<IEnumerable<EmprestimoExibicaoDTO>>(lista));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteLivroEmprestimo>> GetEmprestimo(int id){
            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id);
            if (emprestimo == null) return NotFound("Registro de empréstimo não encontrado.");

            return Ok(emprestimo);
        }


        [HttpPost]
        public async Task<ActionResult> PostEmprestimo(EmprestimoAdicionarDTO dto){
            try{
                var emprestimo = _mapper.Map<ClienteLivroEmprestimo>(dto);
                await _emprestimoRepo.RegistrarEmprestimoAsync(emprestimo);
                return Ok(new { mensagem = "Empréstimo registrado com sucesso!" });
            }catch (Exception ex){
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("renovar/{id}")]
        public async Task<IActionResult> RenovarEmprestimo(int id, [FromBody] int novosDias){
            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id);
            if (emprestimo == null) return NotFound("Empréstimo não encontrado.");

            if (emprestimo.DataDevolucaoReal != null)
                return BadRequest("Não é possível renovar um livro que já foi devolvido.");
            emprestimo.DataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista.AddDays(novosDias);

            await _emprestimoRepo.AtualizarAsync(emprestimo);
            return Ok($"Nova data de devolução: {emprestimo.DataDevolucaoPrevista:dd/MM/yyyy}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DevolverLivro(int id){
            var existe = await _emprestimoRepo.BuscarPorIdAsync(id);
            if (existe == null) return NotFound("Empréstimo não encontrado.");

            await _emprestimoRepo.RegistrarDevolucaoAsync(id);
            return Ok("Devolução realizada e estoque atualizado!");
        }
    }

    public class DatasEmprestimoDTO{
        public DateTime NovaDataPrevista { get; set; }
        public DateTime? NovaDataReal { get; set; }
    }
}