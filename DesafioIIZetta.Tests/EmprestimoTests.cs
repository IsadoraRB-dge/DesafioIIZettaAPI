using Xunit;
using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.Biblioteca;
using DesafioIIZetta.API.Models.GestaoTarefas;
using DesafioIIZetta.API.Repositories.Biblioteca;
using DesafioIIZetta.API.Excecoes;
using System;
using System.Threading.Tasks;

public class EmprestimoTests{
    
    private ControleEmprestimoLivroContext GetDatabase()
    {
        var options = new DbContextOptionsBuilder<ControleEmprestimoLivroContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ControleEmprestimoLivroContext(options);
    }

    private Usuario GetFakeUser() => new Usuario{
        IdUsuario = 1,
        NomeUsuario = "Henrique",
        EmailUsuario = "Henrique@amorzinho.com",
        SenhaUsuario = "123456"
    };

    [Fact]
    public async Task RegistrarEmprestimo_DeveDiminuirEstoque_QuandoSucesso(){
        
        var context = GetDatabase();
        var repo = new EmprestimoRepository(context);
        var user = GetFakeUser();

        var livro = new Livro{
            IdLivro = 1,
            TituloLivro = "Alien",
            AutorLivro = "Alan Dean Foster",
            AnoPublicacaoLivro = 1979,
            QuantidadeEstoqueLivro = 10,
            IdUsuario = 1,
            Usuario = user
        };

        var cliente = new Cliente{
            IdCliente = 1,
            NomeCliente = "Isadora",
            Cpfcliente = "123.456.789-00",
            TelefoneCliente = "22222222",
            EmailCliente = "Isadora@deusa.com",
            EnderecoCliente = "Avenida Elizabeth Bennet",
            IdUsuario = 1,
            Usuario = user
        };

        context.Livros.Add(livro);
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var emprestimo = new ClienteLivroEmprestimo{
            IdLivro = 1,
            IdCliente = 1,
            IdUsuario = 1,
            IdLivroNavigation = livro,
            IdClienteNavigation = cliente,
            UsuarioNavigation = user,
            DataEmprestimo = DateTime.Now,
            DataDevolucaoPrevista = DateTime.Now.AddDays(7)
        };

        await repo.RegistrarEmprestimoAsync(emprestimo);

        var livroNoBanco = await context.Livros.FindAsync(1);
        Assert.Equal(9, livroNoBanco!.QuantidadeEstoqueLivro);
    }

    [Fact]
    public async Task RegistrarDevolucao_DeveAumentarEstoque_ERegistrarDataReal(){
        
        var context = GetDatabase();
        var repo = new EmprestimoRepository(context);
        var user = GetFakeUser();

        var livro = new Livro{
            IdLivro = 10,
            TituloLivro = "Orgulho e Preconceito",
            AutorLivro = "Jane Austen",
            AnoPublicacaoLivro = 1813,
            QuantidadeEstoqueLivro = 5,
            IdUsuario = 1,
            Usuario = user
        };

        var cliente = new Cliente{
            IdCliente = 1,
            NomeCliente = "Isadora",
            Cpfcliente = "123",
            TelefoneCliente = "1",
            EmailCliente = "isadora.com",
            EnderecoCliente = "Rua X",
            IdUsuario = 1,
            Usuario = user
        };

        var emprestimoAtivo = new ClienteLivroEmprestimo{
            Id = 1,
            IdLivro = 10,
            IdCliente = 1,
            IdUsuario = 1,
            IdLivroNavigation = livro,
            IdClienteNavigation = cliente,
            UsuarioNavigation = user,
            DataDevolucaoReal = null
        };

        context.Livros.Add(livro);
        context.Clientes.Add(cliente);
        context.ClienteLivroEmprestimos.Add(emprestimoAtivo);
        await context.SaveChangesAsync();

   
        await repo.RegistrarDevolucaoAsync(1, 1, DateTime.Now);

   
        var livroNoBanco = await context.Livros.FindAsync(10);
        Assert.Equal(6, livroNoBanco!.QuantidadeEstoqueLivro);
    }

    [Fact]
    public async Task RegistrarEmprestimo_DeveLancarExcecao_QuandoEstoqueEsgotado(){
        
        var context = GetDatabase();
        var repo = new EmprestimoRepository(context);
        var user = GetFakeUser();

        var livroSemEstoque = new Livro{
            IdLivro = 2,
            TituloLivro = "Vida",
            AutorLivro = "Autor02",
            AnoPublicacaoLivro = 2024,
            QuantidadeEstoqueLivro = 0,
            IdUsuario = 1,
            Usuario = user
        };

        var cliente = new Cliente{
            IdCliente = 2,
            NomeCliente = "A",
            Cpfcliente = "1",
            TelefoneCliente = "1",
            EmailCliente = "a",
            EnderecoCliente = "a",
            IdUsuario = 1,
            Usuario = user
        };

        context.Livros.Add(livroSemEstoque);
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var emprestimo = new ClienteLivroEmprestimo{
            IdLivro = 2,
            IdCliente = 2,
            IdUsuario = 1,
            IdLivroNavigation = livroSemEstoque,
            IdClienteNavigation = cliente,
            UsuarioNavigation = user
        };

        var ex = await Assert.ThrowsAsync<BibliotecaException>(() => repo.RegistrarEmprestimoAsync(emprestimo));
        Assert.Equal("Este livro não possui exemplares disponíveis no estoque no momento.", ex.Message);
    }
}