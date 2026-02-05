using DesafioIIZetta.API.Models.Biblioteca;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioIIZetta.API.Models.GestaoTarefas;

[Table("Usuario")]
public partial class Usuario{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUsuario { get; set; }

    [Required]
    [StringLength(150)]
    public required string NomeUsuario { get; set; }

    [Required]
    [StringLength(100)]
    public required string EmailUsuario { get; set; }

    [Required]
    [StringLength(255)] 
    public required string SenhaUsuario { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

    [InverseProperty("Usuario")] 
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [InverseProperty("Usuario")] 
    public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();

    [InverseProperty("UsuarioNavigation")] 
    public virtual ICollection<ClienteLivroEmprestimo> Emprestimos { get; set; } = new List<ClienteLivroEmprestimo>();
}