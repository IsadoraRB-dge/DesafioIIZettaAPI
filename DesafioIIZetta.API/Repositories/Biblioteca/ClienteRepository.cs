using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.Biblioteca;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Repositories.Biblioteca{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepo{
        public ClienteRepository(ControleEmprestimoLivroContext context) : base(context) { }

        public async Task<Cliente> BuscarPorCpfAsync(string cpf, int usuarioId){
            // Busca específica por CPF garantindo que o cliente pertença ao usuário logado
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Cpfcliente == cpf && c.IdUsuario == usuarioId);

            if (cliente == null)
                throw new BibliotecaException($"Cliente com CPF {cpf} não encontrado.");

            return cliente;
        }

        // Sobrescreve o método da Base para incluir os relacionamentos (Eager Loading)
        // Isso permite retornar o cliente já com seu histórico de empréstimos e os dados dos livros
        public override async Task<Cliente> BuscarPorIdAsync(int id, int usuarioId){
            var cliente = await _context.Clientes
                .Include(c => c.ClienteLivroEmprestimos)
                .ThenInclude(e => e.IdLivroNavigation)
                .FirstOrDefaultAsync(c => c.IdCliente == id && c.IdUsuario == usuarioId);

        if (cliente == null)
            throw new BibliotecaException("Cliente não encontrado ou você não tem permissão para acessá-lo.");

            return cliente;
        }

        public async Task<bool> ExisteCpfAsync(string cpf){
            return await _context.Clientes.AnyAsync(c => c.Cpfcliente == cpf);
        }
    }
}