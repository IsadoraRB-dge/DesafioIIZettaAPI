using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.Biblioteca;
using DesafioIIZetta.API.Models.GestaoTarefas;
using DesafioIIZetta.API.Repositories.Biblioteca;
using DesafioIIZetta.API.Excecoes;

namespace DesafioIIZetta.Tests;

public class ClienteRepositoryTests{
    private ControleEmprestimoLivroContext GetDatabase()
    {
        var options = new DbContextOptionsBuilder<ControleEmprestimoLivroContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ControleEmprestimoLivroContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private Usuario GetFakeUser(int id, string nome) => new Usuario{
        IdUsuario = id,
        NomeUsuario = nome,
        EmailUsuario = $"{nome.ToLower()}@gmail.com",
        SenhaUsuario = "123456"
    };

    [Fact]
    public async Task BuscarPorCpfAsync_DeveRetornarCliente_QuandoCpfPertenceAoUsuario(){
        var context = GetDatabase();
        var repo = new ClienteRepository(context);
        var user = GetFakeUser(1, "Henrique");

        var cliente = new Cliente{
            IdCliente = 1,
            NomeCliente = "Isadora",
            Cpfcliente = "123.456.789-00",
            TelefoneCliente = "111",
            EmailCliente = "isa@isa.com",
            EnderecoCliente = "Rua X",
            IdUsuario = 1,
            Usuario = user
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var resultado = await repo.BuscarPorCpfAsync("123.456.789-00", 1);

        Assert.NotNull(resultado);
        Assert.Equal("Isadora", resultado.NomeCliente);
    }

    [Fact]
    public async Task BuscarPorIdAsync_DeveLancarExcecao_QuandoUsuarioTentarAcessarDadosDeOutro(){
        var context = GetDatabase();
        var repo = new ClienteRepository(context);

        var userDono = GetFakeUser(1, "Dono");
        context.Usuarios.Add(userDono);

        var clientePrivado = new Cliente{
            IdCliente = 50,
            NomeCliente = "Genis",
            Cpfcliente = "000",
            TelefoneCliente = "0",
            EmailCliente = "Genis@gmail.com",
            EnderecoCliente = "Rua G",
            IdUsuario = 1,
            Usuario = userDono
        };

        context.Clientes.Add(clientePrivado);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<BibliotecaException>(() => repo.BuscarPorIdAsync(50, 2));
    }

    [Fact]
    public async Task ListarTodosAsync_DeveRetornarApenasClientesDoUsuarioLogado(){
        var context = GetDatabase();
        var repo = new ClienteRepository(context);
        var u1 = GetFakeUser(1, "User1");
        var u2 = GetFakeUser(2, "User2");

        context.Clientes.Add(new Cliente { IdCliente = 1, NomeCliente = "C1", Cpfcliente = "1", TelefoneCliente = "1", EmailCliente = "a", EnderecoCliente = "a", IdUsuario = 1, Usuario = u1 });
        context.Clientes.Add(new Cliente { IdCliente = 2, NomeCliente = "C2", Cpfcliente = "2", TelefoneCliente = "1", EmailCliente = "b", EnderecoCliente = "b", IdUsuario = 1, Usuario = u1 });
        context.Clientes.Add(new Cliente { IdCliente = 3, NomeCliente = "C3", Cpfcliente = "3", TelefoneCliente = "1", EmailCliente = "c", EnderecoCliente = "c", IdUsuario = 2, Usuario = u2 });

        await context.SaveChangesAsync();

        var lista = await repo.ListarTodosAsync(1);

        Assert.Equal(2, lista.Count);
        Assert.All(lista, c => Assert.Equal(1, c.IdUsuario));
    }

    [Fact]
    public async Task AdicionarAsync_DevePersistirClienteNoBanco(){
        
        var context = GetDatabase();
        var repo = new ClienteRepository(context);
        var user = GetFakeUser(1, "Henrique");
        context.Usuarios.Add(user);
        await context.SaveChangesAsync();

        var novoCliente = new Cliente{
            NomeCliente = "Rafa",
            Cpfcliente = "444",
            TelefoneCliente = "9955",
            EmailCliente = "Rafa@teste.com",
            EnderecoCliente = "Rua da Rafolandia",
            IdUsuario = 1,
            Usuario = user
        };

       
        await repo.AdicionarAsync(novoCliente);

       
        var clienteNoBanco = await context.Clientes
            .AsNoTracking() 
            .FirstOrDefaultAsync(c => c.Cpfcliente == "444");

        Assert.NotNull(clienteNoBanco);
        Assert.Equal("Rafa", clienteNoBanco.NomeCliente);
    }



    [Fact]
    public async Task ExisteCpfAsync_DeveRetornarTrue_QuandoCpfJaCadastrado(){
   
        var context = GetDatabase();
        var repo = new ClienteRepository(context);
        var user = GetFakeUser(1, "Henrique");

        var clienteExistente = new Cliente{
            NomeCliente = "Original",
            Cpfcliente = "123.456.789-00",
            TelefoneCliente = "0",
            EmailCliente = "a@a.com",
            EnderecoCliente = "Rua X",
            IdUsuario = 1,
            Usuario = user
        };

        context.Clientes.Add(clienteExistente);
        await context.SaveChangesAsync();

        var existe = await repo.ExisteCpfAsync("123.456.789-00");

      
        Assert.True(existe);
    }

    [Fact]
    public async Task ExisteCpfAsync_DeveRetornarFalse_QuandoCpfNaoExiste(){
        var context = GetDatabase();
        var repo = new ClienteRepository(context);

        var existe = await repo.ExisteCpfAsync("999.999.999-99");

        Assert.False(existe);
    }
}