using AutoMapper;
using DesafioIIZetta.API.DTOs.Biblioteca.Cliente;
using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models.Biblioteca;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIIZetta.API.Controllers.Biblioteca{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase{
        private readonly IClienteRepo _clienteRepo;
        private readonly IMapper _mapper;

        public ClienteController(IClienteRepo clienteRepo, IMapper mapper){
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteExibicaoDTO>>> GetClientes(){
            var clientes = await _clienteRepo.ListarTodosAsync();
            var clientesDto = _mapper.Map<IEnumerable<ClienteExibicaoDTO>>(clientes);
            return Ok(clientesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDetalhesDTO>> GetCliente(int id){
            var cliente = await _clienteRepo.BuscarPorIdAsync(id);
            if (cliente == null){
                return NotFound("Cliente não encontrado.");
            }
            return Ok(_mapper.Map<ClienteDetalhesDTO>(cliente));
        }

        [HttpPost]
        public async Task<ActionResult<ClienteExibicaoDTO>> PostCliente(ClienteAdicionarDTO clienteDto){
            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _clienteRepo.AdicionarAsync(cliente);

            var clienteRetorno = _mapper.Map<ClienteExibicaoDTO>(cliente);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, clienteRetorno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteAtualizarDTO clienteDto){
            if (id != clienteDto.Id){
                return BadRequest("O ID enviado não corresponde ao ID do objeto.");
            }
            var clienteBanco = await _clienteRepo.BuscarPorIdAsync(id);

            if (clienteBanco == null){
                return NotFound("Cliente não encontrado para atualização.");
            }
            _mapper.Map(clienteDto, clienteBanco);

            await _clienteRepo.AtualizarAsync(clienteBanco);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id){
            var cliente = await _clienteRepo.BuscarPorIdAsync(id);
            if (cliente == null){
                return NotFound();
            }
            await _clienteRepo.ExcluirAsync(id);
            return NoContent();
        }
    }
}