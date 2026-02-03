
using AutoMapper;
using DesafioIIZetta.API.DTOs.Livro;
using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIIZetta.API.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase{
        private readonly ILivroRepo _livroRepo;
        private readonly IMapper _mapper;

        public LivroController(ILivroRepo livroRepo, IMapper mapper){
            _livroRepo = livroRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroExibicaoDTO>>> GetLivro(){
            var livros = await _livroRepo.ListarTodosAsync();
            return Ok(_mapper.Map<IEnumerable<LivroExibicaoDTO>>(livros));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LivroExibicaoDTO>> GetLivro(int id){
            var livro = await _livroRepo.BuscarPorIdAsync(id);

            if (livro == null){
                return NotFound("Livro não encontrado.");
            }

            return Ok(_mapper.Map<LivroExibicaoDTO>(livro));
        }

        [HttpPost]
        public async Task<ActionResult<LivroExibicaoDTO>> PostLivro(LivroAdicionarDTO livroDto){

            var livro = _mapper.Map<Livro>(livroDto);

            await _livroRepo.AdicionarAsync(livro);
            var retornoDto = _mapper.Map<LivroExibicaoDTO>(livro);

            return CreatedAtAction(nameof(GetLivro), new { id = livro.IdLivro }, retornoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, LivroAtualizarDTO livroDto){
            if (id != livroDto.Id){
                return BadRequest("O ID enviado não corresponde ao ID do objeto.");
            }

            var livroBanco = await _livroRepo.BuscarPorIdAsync(id);
            if (livroBanco == null){
                return NotFound("Livro não encontrado.");
            }

            _mapper.Map(livroDto, livroBanco);

            await _livroRepo.AtualizarAsync(livroBanco);
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
