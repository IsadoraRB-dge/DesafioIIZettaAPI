using AutoMapper;
using DesafioIIZetta.API.DTOs.Livro;
using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DesafioIIZetta.API.Controllers{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase{
        private readonly ILivroRepo _livroRepo;
        private readonly IMapper _mapper;

        public LivroController(ILivroRepo livroRepo, IMapper mapper){
            _livroRepo = livroRepo;
            _mapper = mapper;
        }

        private int ObterUsuarioLogadoId(){
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(idClaim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroExibicaoDTO>>> GetLivro(){
            var usuarioId = ObterUsuarioLogadoId();
            var livros = await _livroRepo.ListarTodosAsync(usuarioId);
                return Ok(_mapper.Map<IEnumerable<LivroExibicaoDTO>>(livros));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LivroExibicaoDTO>> GetLivro(int id){
            var usuarioId = ObterUsuarioLogadoId();
            var livro = await _livroRepo.BuscarPorIdAsync(id, usuarioId);

            return Ok(_mapper.Map<LivroExibicaoDTO>(livro));
        }

        [HttpPost]
        public async Task<ActionResult<LivroExibicaoDTO>> PostLivro(LivroAdicionarDTO livroDto){
            var livro = _mapper.Map<Livro>(livroDto);
            // Define a propriedade de navegação para o usuário logado antes de persistir no banco
            livro.IdUsuario = ObterUsuarioLogadoId();
            await _livroRepo.AdicionarAsync(livro);
            var retornoDto = _mapper.Map<LivroExibicaoDTO>(livro);
                // Retorna o Status 201 Created com o cabeçalho 'Location' apontando para o GET do novo recurso
                return CreatedAtAction(nameof(GetLivro), new { id = livro.IdLivro }, retornoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, LivroAtualizarDTO livroDto){
            // Validação para evitar requisições inconsistentes
            if (id != livroDto.Id){
                throw new BibliotecaException("O ID informado na URL não coincide com o ID do livro enviado.");
            }

            var usuarioId = ObterUsuarioLogadoId();
            var livroBanco = await _livroRepo.BuscarPorIdAsync(id, usuarioId);

            if (livroBanco == null)
                throw new BibliotecaException("Livro não localizado.");

            _mapper.Map(livroDto, livroBanco);
            await _livroRepo.AtualizarAsync(livroBanco);
            // Embora o padrão REST sugira NoContent, os retornos Ok com mensagem facilita a integração com Front-ends
            return Ok(new { mensagem = "Livro atualizado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivro(int id){
            var usuarioId = ObterUsuarioLogadoId();
            var livro = await _livroRepo.BuscarPorIdAsync(id, usuarioId);
            // Se o livro possuir empréstimos ativos, a restrição de chave estrangeira no banco garantirá a integridade
            await _livroRepo.ExcluirAsync(livro);
                return Ok(new { mensagem = "O livro foi removido com sucesso!" });
        }
    }
}