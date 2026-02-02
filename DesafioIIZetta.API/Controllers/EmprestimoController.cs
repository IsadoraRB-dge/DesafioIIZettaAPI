using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIIZetta.API.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase{
        private readonly IEmprestimoRepo _emprestimoRepo;

        public EmprestimoController(IEmprestimoRepo emprestimoRepo){
            _emprestimoRepo = emprestimoRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteLivroEmprestimo>>> GetEmprestimo(){
            var lista = await _emprestimoRepo.ListarTodosAsync();
            return Ok(lista);
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteLivroEmprestimo>> GetEmprestimo(int id){
            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id);
            if (emprestimo == null) return NotFound("Registro de empréstimo não encontrado.");

            return Ok(emprestimo);
        }

   
        [HttpPost]
        public async Task<ActionResult> PostEmprestimo(ClienteLivroEmprestimo emprestimo){
            try{
                await _emprestimoRepo.RegistrarEmprestimoAsync(emprestimo);
                return Ok("Empréstimo registrado com sucesso! O estoque do livro foi atualizado.");
            }catch (Exception ex){
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("atualizar-datas/{id}")]
        public async Task<IActionResult> AtualizarDatas(int id, [FromBody] DatasEmprestimoDTO datas){
            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id);
            if (emprestimo == null) return NotFound("Empréstimo não encontrado.");

            emprestimo.DataDevolucaoPrevista = datas.NovaDataPrevista;

            if (emprestimo.DataDevolucaoReal == null && datas.NovaDataReal != null){
                await _emprestimoRepo.RegistrarDevolucaoAsync(id);
                return Ok("Devolução registrada e estoque atualizado!");
            }

            emprestimo.DataDevolucaoReal = datas.NovaDataReal;
            await _emprestimoRepo.AtualizarAsync(emprestimo);

            return Ok("Datas atualizadas com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DevolverLivro(int id){
            var existe = await _emprestimoRepo.BuscarPorIdAsync(id);
            if (existe == null) return NotFound("ID de empréstimo inválido.");

            await _emprestimoRepo.RegistrarDevolucaoAsync(id);
            return Ok("Devolução realizada com sucesso! O livro retornou ao estoque.");
        }
    }

    public class DatasEmprestimoDTO{
        public DateTime NovaDataPrevista { get; set; }
        public DateTime? NovaDataReal { get; set; }
    }
}