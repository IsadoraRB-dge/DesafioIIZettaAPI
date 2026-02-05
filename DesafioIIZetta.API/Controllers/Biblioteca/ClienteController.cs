using AutoMapper;
using DesafioIIZetta.API.DTOs.Biblioteca.Cliente;
using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models.Biblioteca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DesafioIIZetta.API.Controllers.Biblioteca{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase{
        private readonly IClienteRepo _clienteRepo;
        private readonly IMapper _mapper;

        public ClienteController(IClienteRepo clienteRepo, IMapper mapper){
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        // Extrai o ID do usuário autenticado a partir do Token JWT.
        // Esse ID é essencial para garantir a segregação de dados entre usuários.
        private int ObterUsuarioLogadoId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteExibicaoDTO>>> GetClientes(){
            var usuarioId = ObterUsuarioLogadoId();
            // Filtra a listagem para que um usuário nunca veja os clientes de outro
            var clientes = await _clienteRepo.ListarTodosAsync(usuarioId);
            var clientesDto = _mapper.Map<IEnumerable<ClienteExibicaoDTO>>(clientes);
                return Ok(clientesDto);
        }

   
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDetalhesDTO>> GetCliente(int id){
            var usuarioId = ObterUsuarioLogadoId();
            var cliente = await _clienteRepo.BuscarPorIdAsync(id, usuarioId);

                return Ok(_mapper.Map<ClienteDetalhesDTO>(cliente));
        }

        [HttpGet("buscar-por-cpf/{cpf}")]
        public async Task<ActionResult<ClienteDetalhesDTO>> GetClientePorCpf(string cpf){
            var usuarioId = ObterUsuarioLogadoId();

            var cliente = await _clienteRepo.BuscarPorCpfAsync(cpf, usuarioId);

            // Reaproveitamento do ClienteDetalhesDTO, pois ele já contém toda a estrutura necessária
            return Ok(_mapper.Map<ClienteDetalhesDTO>(cliente));
        }


        [HttpPost]
        public async Task<ActionResult<ClienteExibicaoDTO>> PostCliente(ClienteAdicionarDTO clienteDto){
            // Garante que não existam dois clientes com o mesmo CPF
            if (await _clienteRepo.ExisteCpfAsync(clienteDto.Cpf)){
                throw new BibliotecaException("Este CPF já está cadastrado para outro cliente.");
            }
            var cliente = _mapper.Map<Cliente>(clienteDto);
            // Vincula o novo cliente ao usuário que está logado no sistema
            cliente.IdUsuario = ObterUsuarioLogadoId();
            await _clienteRepo.AdicionarAsync(cliente);
            var clienteRetorno = _mapper.Map<ClienteExibicaoDTO>(cliente);
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, clienteRetorno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteAtualizarDTO clienteDto){
            // Validação de segurança para garantir que o ID da URL é o mesmo do objeto enviado
            if (id != clienteDto.Id){
                throw new BibliotecaException("O ID enviado na URL não corresponde ao ID do corpo da requisição.");
            }

            var usuarioId = ObterUsuarioLogadoId();
            var clienteBanco = await _clienteRepo.BuscarPorIdAsync(id, usuarioId);

            if (clienteBanco == null){
                throw new BibliotecaException("Cliente não encontrado para este usuário.");
            }

            _mapper.Map(clienteDto, clienteBanco);
            await _clienteRepo.AtualizarAsync(clienteBanco);
                return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id){
            var usuarioId = ObterUsuarioLogadoId();
            var cliente = await _clienteRepo.BuscarPorIdAsync(id, usuarioId);

            if (cliente == null)
                throw new BibliotecaException("Cliente não encontrado para exclusão.");

            await _clienteRepo.ExcluirAsync(cliente);
                return Ok(new { mensagem = "Cliente removido com sucesso!" });
        }
    }
}