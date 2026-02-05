using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using DesafioIIZetta.API.Repositories;
using DesafioIIZetta.API.Excecoes;


namespace DesafioIIZetta.Tests;

public class UsuarioRepositoryTests{
    private ControleEmprestimoLivroContext GetDatabase()
    {
        var options = new DbContextOptionsBuilder<ControleEmprestimoLivroContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ControleEmprestimoLivroContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task BuscarPorEmailAsync_DeveRetornarUsuario_QuandoEmailExiste(){
        
        var context = GetDatabase();
        var repo = new UsuarioRepository(context);
        var emailTeste = "henrique@gmail.com";

        context.Usuarios.Add(new Usuario{
            NomeUsuario = "Henrique",
            EmailUsuario = emailTeste,
            SenhaUsuario = "hash_senha_123"
        });
        await context.SaveChangesAsync();

   
        var usuario = await repo.BuscarPorEmailAsync(emailTeste);

    
        Assert.NotNull(usuario);
        Assert.Equal("Henrique", usuario!.NomeUsuario);
    }

    [Fact]
    public async Task UsuarioJaExisteAsync_DeveLancarTarefaException_QuandoEmailJaEstaCadastrado(){
        
        var context = GetDatabase();
        var repo = new UsuarioRepository(context);
        var emailDuplicado = "duplicado@gmail.com";

        context.Usuarios.Add(new Usuario{
            NomeUsuario = "Usuario Original",
            EmailUsuario = emailDuplicado,
            SenhaUsuario = "123"
        });
        await context.SaveChangesAsync();

     
        var ex = await Assert.ThrowsAsync<TarefaException>(() => repo.UsuarioJaExisteAsync(emailDuplicado));

        Assert.Equal("Este e-mail já está cadastrado no sistema.", ex.Message);
    }

    [Fact]
    public async Task AdicionarAsync_DeveSalvarNovoUsuarioNoBanco(){
        
        var context = GetDatabase();
        var repo = new UsuarioRepository(context);
        var novoUser = new Usuario{
            NomeUsuario = "Novo Dev",
            EmailUsuario = "dev@zetta.com",
            SenhaUsuario = "senhaForte123"
        };

        await repo.AdicionarAsync(novoUser);
        await repo.SalvarAlteracoesAsync();

        var usuarioNoDb = await context.Usuarios.FirstOrDefaultAsync(u => u.EmailUsuario == "dev@zetta.com");
        Assert.NotNull(usuarioNoDb);
        Assert.Equal("Novo Dev", usuarioNoDb!.NomeUsuario);
    }
}