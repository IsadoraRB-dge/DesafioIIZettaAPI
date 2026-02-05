using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using DesafioIIZetta.API.Repositories.Biblioteca;
using DesafioIIZetta.API.Excecoes;

namespace DesafioIIZetta.Tests;

public class LivroRepositoryTests{
    private ControleEmprestimoLivroContext GetDatabase(){
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
    public async Task BuscarPorIdAsync_DeveLancarExcecao_QuandoLivroPertenceAOutroUsuario(){
        var context = GetDatabase();
        var repo = new LivroRepository(context);
   
        var userDono = GetFakeUser(1, "Dono");
        var userInvasor = GetFakeUser(2, "Invasor");
        context.Usuarios.AddRange(userDono, userInvasor);

  
        var livroPrivado = new Livro{
            IdLivro = 100,
            TituloLivro = "Gatinhos Fofos",
            AutorLivro = "Adriela",
            AnoPublicacaoLivro = 2023,
            QuantidadeEstoqueLivro = 10,
            IdUsuario = 1,
            Usuario = userDono
        };

        context.Livros.Add(livroPrivado);
        await context.SaveChangesAsync();


        var ex = await Assert.ThrowsAsync<BibliotecaException>(() => repo.BuscarPorIdAsync(100, 2));

        Assert.Equal("Livro não encontrado ou você não possui permissão para acessá-lo.", ex.Message);
    }

    [Fact]
    public async Task ListarTodosAsync_DeveRetornarApenasLivrosDoUsuarioLogado(){
        
        var context = GetDatabase();
        var repo = new LivroRepository(context);
        var u1 = GetFakeUser(1, "Henrique");
        var u2 = GetFakeUser(2, "Zetta");
        context.Usuarios.AddRange(u1, u2);

 
        context.Livros.Add(new Livro { TituloLivro = "C# Avançado", AutorLivro = "A", QuantidadeEstoqueLivro = 1, IdUsuario = 1, Usuario = u1 });
        context.Livros.Add(new Livro { TituloLivro = "EF Core Guide", AutorLivro = "B", QuantidadeEstoqueLivro = 1, IdUsuario = 1, Usuario = u1 });


        context.Livros.Add(new Livro { TituloLivro = "Livro Alheio", AutorLivro = "C", QuantidadeEstoqueLivro = 1, IdUsuario = 2, Usuario = u2 });

        await context.SaveChangesAsync();

   
        var lista = await repo.ListarTodosAsync(1);

        Assert.Equal(2, lista.Count);
        Assert.All(lista, l => Assert.Equal(1, l.IdUsuario));
    }

    [Fact]
    public async Task AdicionarAsync_DeveSalvarLivroCorretamente(){
      
        var context = GetDatabase();
        var repo = new LivroRepository(context);
        var user = GetFakeUser(1, "Henrique");
        context.Usuarios.Add(user);
        await context.SaveChangesAsync();

        var novoLivro = new Livro{
            TituloLivro = "Clean Code",
            AutorLivro = "Robert Martin",
            AnoPublicacaoLivro = 2008,
            QuantidadeEstoqueLivro = 5,
            IdUsuario = 1,
            Usuario = user
        };

        
        await repo.AdicionarAsync(novoLivro);

        var livroNoBanco = await context.Livros
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.TituloLivro == "Clean Code");

        Assert.NotNull(livroNoBanco);
        Assert.Equal("Robert Martin", livroNoBanco!.AutorLivro);
        Assert.Equal(5, livroNoBanco.QuantidadeEstoqueLivro);
    }
}