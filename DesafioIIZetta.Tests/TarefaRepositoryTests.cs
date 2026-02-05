using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Models.GestaoTarefas;
using DesafioIIZetta.API.Repositories; 
using DesafioIIZetta.API.Excecoes;


namespace DesafioIIZetta.Tests;

public class TarefaRepositoryTests{
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
        EmailUsuario = $"{nome.ToLower()}@zetta.com",
        SenhaUsuario = "123456"
    };

    [Fact]
    public async Task ListarTarefasAsync_DeveFiltrarPorStatusEPrioridade(){
       
        var context = GetDatabase();
        var repo = new TarefaRepository(context);
        var user = GetFakeUser(1, "Henrique");
        context.Usuarios.Add(user);

        context.Tarefas.AddRange(
            new Tarefa { IdTarefa = 1, NomeTarefa = "T1", DescricaoTarefa = "D1", StatusTarefa = "Pendente", Prioridade = "Alta", IdUsuario = 1, IdUsuarioNavigation = user },
            new Tarefa { IdTarefa = 2, NomeTarefa = "T2", DescricaoTarefa = "D2", StatusTarefa = "Concluida", Prioridade = "Alta", IdUsuario = 1, IdUsuarioNavigation = user },
            new Tarefa { IdTarefa = 3, NomeTarefa = "T3", DescricaoTarefa = "D3", StatusTarefa = "Pendente", Prioridade = "Baixa", IdUsuario = 1, IdUsuarioNavigation = user }
        );
        await context.SaveChangesAsync();

      
        var resultado = await repo.ListarTarefasAsync(1, "Pendente", "Alta");

        Assert.Single(resultado);
        var tarefa = resultado.First();
        Assert.Equal("T1", tarefa.NomeTarefa);
    }

    [Fact]
    public async Task BuscarPorIdAsync_DeveLancarTarefaException_QuandoNaoEncontrarOuAcessoNegado(){
      
        var context = GetDatabase();
        var repo = new TarefaRepository(context);
        var user = GetFakeUser(1, "Dono");
        context.Usuarios.Add(user);

        context.Tarefas.Add(new Tarefa{
            IdTarefa = 10,
            NomeTarefa = "Organizar Livros",
            DescricaoTarefa = "Organizar todos os livros da estante B",
            StatusTarefa = "Pendente",
            Prioridade = "Media",
            IdUsuario = 1,
            IdUsuarioNavigation = user
        });
        await context.SaveChangesAsync();

        var ex = await Assert.ThrowsAsync<TarefaException>(() => repo.BuscarPorIdAsync(10, 99));
        Assert.Contains("não tem permissão", ex.Message);
    }

    [Fact]
    public async Task AdicionarAsync_DeveAdicionarTarefaMasNaoSalvarAinda(){
      
        var context = GetDatabase();
        var repo = new TarefaRepository(context);
        var user = GetFakeUser(1, "Henrique");
        context.Usuarios.Add(user);
        await context.SaveChangesAsync();

        var novaTarefa = new Tarefa{
            NomeTarefa = "Ligar para o Cliente X",
            DescricaoTarefa = "Avisar da chegada da nova coleção de GOT",
            StatusTarefa = "Pendente",
            Prioridade = "Alta",
            IdUsuario = 1,
            IdUsuarioNavigation = user
        };

      
        await repo.AdicionarAsync(novaTarefa);

   
        var antesDeSalvar = await context.Tarefas.CountAsync();

        await repo.SalvarAlteracoesAsync();
        var depoisDeSalvar = await context.Tarefas.CountAsync();

        Assert.Equal(1, depoisDeSalvar);
    }

    [Fact]
    public async Task DeletarAsync_DeveRemoverTarefaDoBanco(){
    
        var context = GetDatabase();
        var repo = new TarefaRepository(context);
        var user = GetFakeUser(1, "Henrique");
        context.Usuarios.Add(user);

        var tarefa = new Tarefa { IdTarefa = 1, NomeTarefa = "Atualizar lista de clientes", DescricaoTarefa = "Adicionar os novos clientes", StatusTarefa = "Pendente", IdUsuario = 1, IdUsuarioNavigation = user };
        context.Tarefas.Add(tarefa);
        await context.SaveChangesAsync();

        await repo.DeletarAsync(tarefa);
        await repo.SalvarAlteracoesAsync();

      
        var existe = await context.Tarefas.AnyAsync(t => t.IdTarefa == 1);
        Assert.False(existe);
    }
}