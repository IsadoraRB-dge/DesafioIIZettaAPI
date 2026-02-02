using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIIZetta.API.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase{
        private readonly IClienteRepo _clienteRepo;

        public ClienteController(IClienteRepo clienteRepo){
            _clienteRepo = clienteRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes(){
            var clientes = await _clienteRepo.ListarTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id){
            var cliente = await _clienteRepo.BuscarPorIdAsync(id);

            if (cliente == null){
                return NotFound("Cliente não encontrado.");
            }

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente){
            await _clienteRepo.AdicionarAsync(cliente);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente){
            if (id != cliente.IdCliente){
                return BadRequest("O ID enviado não corresponde ao ID do objeto.");
            }

            await _clienteRepo.AtualizarAsync(cliente);
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