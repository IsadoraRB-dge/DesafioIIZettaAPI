using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIIZetta.API.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase{
        private readonly ILivroRepo _livroRepo;

        public LivroController(ILivroRepo livroRepo){
            _livroRepo = livroRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivro(){
            var livros = await _livroRepo.ListarTodosAsync();
            return Ok(livros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> GetLivro(int id){
            var livro = await _livroRepo.BuscarPorIdAsync(id);

            if (livro == null){
                return NotFound("Livro não encontrado.");
            }

            return Ok(livro);
        }

        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(Livro livro){
            await _livroRepo.AdicionarAsync(livro);
            return CreatedAtAction(nameof(GetLivro), new { id = livro.IdLivro }, livro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, Livro livro){
            if (id != livro.IdLivro){
                return BadRequest("O ID enviado não corresponde ao ID do objeto.");
            }

            await _livroRepo.AtualizarAsync(livro);
            return NoContent();
        }

 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(int id){
            var livro = await _livroRepo.BuscarPorIdAsync(id);
            if (livro == null){
                return NotFound("Livro não existe no sistema.");
            }

            await _livroRepo.ExcluirAsync(id);
            return NoContent();
        }
    }
}
