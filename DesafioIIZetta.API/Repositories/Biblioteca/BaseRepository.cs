using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Models;
using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Excecoes;


// O uso de Generics (C) permite que este repositório sirva para qualquer entidade do sistema,
// centralizando as operações de CRUD e garantindo a padronização.
public class BaseRepository<C> : IBaseRepo<C> where C : class{
    protected readonly ControleEmprestimoLivroContext _context;

    public BaseRepository(ControleEmprestimoLivroContext context){
        _context = context;
    }

    public async Task<List<C>> ListarTodosAsync(int usuarioId){
        // Utilizo Shadow Properties (EF.Property) para filtrar pelo IdUsuario
        return await _context.Set<C>()
            .Where(x => EF.Property<int>(x, "IdUsuario") == usuarioId)
            .ToListAsync();
    }

    public virtual async Task<C> BuscarPorIdAsync(int id, int usuarioId){
        //É feita uma descoberta dinamica sobre qual é a Chave Primária da classe 'C'
        // para que a busca funcione com qualquer tabela (Cliente, Livro, etc).
        var entityType = _context.Model.FindEntityType(typeof(C));
        var primaryKey = entityType?.FindPrimaryKey();
        var nomeChave = primaryKey?.Properties.Select(p => p.Name).Single();

        if (string.IsNullOrEmpty(nomeChave)){
            throw new Exception($"Não foi possível determinar a chave primária para a entidade {typeof(C).Name}.");
        }
        var entidade = await _context.Set<C>()
            .FirstOrDefaultAsync(x =>
                EF.Property<int>(x, "IdUsuario") == usuarioId &&
                EF.Property<int>(x, nomeChave) == id);

        if (entidade == null)
            throw new BibliotecaException($"O registro com ID {id} não foi encontrado.");

        return entidade;
    }

    public async Task AdicionarAsync(C entidade){
        // Marca a entidade como Modified para que o EF gere o comando UPDATE SQL.
        await _context.Set<C>().AddAsync(entidade);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(C entidade){
        _context.Set<C>().Update(entidade);
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAsync(C entidade){
        _context.Set<C>().Remove(entidade);
        await _context.SaveChangesAsync();
    }
}