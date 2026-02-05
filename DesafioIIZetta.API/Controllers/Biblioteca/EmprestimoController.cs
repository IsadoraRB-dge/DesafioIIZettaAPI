using AutoMapper;
using DesafioIIZetta.API.DTOs.Biblioteca.Emprestimo;
using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DesafioIIZetta.API.Controllers.Biblioteca{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase{
        private readonly IEmprestimoRepo _emprestimoRepo;
        private readonly IMapper _mapper;
        private readonly IMultaService _multaService;

        public EmprestimoController(IEmprestimoRepo emprestimoRepo, IMapper mapper, IMultaService multaService)
        {
            _emprestimoRepo = emprestimoRepo;
            _mapper = mapper;
            _multaService = multaService;
        }

        private int ObterUsuarioLogadoId(){
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(idClaim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmprestimoExibicaoDTO>>> GetEmprestimo(){
            var usuarioId = ObterUsuarioLogadoId();
            var lista = await _emprestimoRepo.ListarTodosAsync(usuarioId);
                return Ok(_mapper.Map<IEnumerable<EmprestimoExibicaoDTO>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmprestimoExibicaoDTO>> GetEmprestimo(int id){
            var usuarioId = ObterUsuarioLogadoId();
            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id, usuarioId);

                return Ok(_mapper.Map<EmprestimoExibicaoDTO>(emprestimo));
        }

        [HttpPost]
        public async Task<ActionResult> PostEmprestimo(EmprestimoAdicionarDTO dto){
            
                var emprestimo = _mapper.Map<ClienteLivroEmprestimo>(dto);
            // Garante que o registro do empréstimo fique vinculado ao usuário(bibliotecário) logado
            emprestimo.IdUsuario = ObterUsuarioLogadoId();
            // A lógica de negócio (como reduzir estoque do livro) é delegada ao repositório 
            await _emprestimoRepo.RegistrarEmprestimoAsync(emprestimo);
                return Ok(new { mensagem = "Empréstimo registrado com sucesso!" });
        }

        [HttpPatch("renovar/{id}")] 
        public async Task<IActionResult> RenovarEmprestimo(int id, [FromBody] EmprestimoRenovarDTO dto){
            var usuarioId = ObterUsuarioLogadoId();
            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id, usuarioId);
            // Impede a renovação de itens que já retornaram ao acervo
            if (emprestimo.DataDevolucaoReal != null)
                throw new BibliotecaException("Não é possível renovar um livro que já foi devolvido.");
                //Bloqueia renovações se o livro já estiver com data de entrega atrasada
                if (emprestimo.DataDevolucaoPrevista < DateTime.Now)
                    throw new BibliotecaException("Não é possível renovar um empréstimo em atraso. Devolva e pague a multa primeiro.");
            // Atualiza a nova data de previsão somando os dias permitidos na regra de renovação
            emprestimo.DataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista.AddDays(dto.DiasAdicionais);

            await _emprestimoRepo.AtualizarAsync(emprestimo);

            return Ok(new
            {
                mensagem = "Renovação concluída!",
                novaData = emprestimo.DataDevolucaoPrevista.ToString("dd/MM/yyyy")
            });
        }

        [HttpPatch("devolver/{id}")] 
        public async Task<IActionResult> DevolverLivro(int id, [FromBody] EmprestimoDevolucaoDTO dto)
        {
            var usuarioId = ObterUsuarioLogadoId();

            var emprestimo = await _emprestimoRepo.BuscarPorIdAsync(id, usuarioId);
            if (emprestimo == null) return NotFound("Empréstimo não encontrado.");
            // Processa a devolução no banco e ajusta o status do livro para "Disponível"
            await _emprestimoRepo.RegistrarDevolucaoAsync(id, usuarioId, dto.DataDevolucaoManual);
            // Cálculo de Multa: Utiliza um serviço especializado para aplicar a regra financeira de atraso
            var multa = _multaService.CalcularValorMulta(emprestimo.DataDevolucaoPrevista, emprestimo.DataDevolucaoReal);
            return Ok(new
            {
                mensagem = "Devolução realizada com sucesso!",
                dataDevolucao = emprestimo.DataDevolucaoReal,
                valorMultaCobrado = multa
            });
        }
    }
}