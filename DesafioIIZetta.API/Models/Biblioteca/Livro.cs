using DesafioIIZetta.API.Models.GestaoTarefas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioIIZetta.API.Models;

[Table("Livro")]
public partial class Livro{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdLivro { get; set; }

    [Required]
    [StringLength(150)]
    public required string TituloLivro { get; set; }

    [Required]
    [StringLength(150)]
    public required string AutorLivro { get; set; }

    public int AnoPublicacaoLivro { get; set; }

    public int QuantidadeEstoqueLivro { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Livros")]
    public required virtual Usuario Usuario { get; set; }

    [InverseProperty("IdLivroNavigation")]
    public virtual ICollection<ClienteLivroEmprestimo> ClienteLivroEmprestimos { get; set; } = new List<ClienteLivroEmprestimo>();
}