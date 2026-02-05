using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioIIZetta.API.Models.Biblioteca;

[Table("Cliente")]
[Index(nameof(Cpfcliente), IsUnique = true)]
public partial class Cliente{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCliente { get; set; }

    [Required]
    [StringLength(150)]
    public required string NomeCliente { get; set; }

    [Required]
    [Column("CPFCliente")]
    [StringLength(14)]
    public required string Cpfcliente { get; set; }

    [Required]
    [StringLength(20)]
    public required string TelefoneCliente { get; set; }

    [Required]
    [StringLength(100)]
    public required string EmailCliente { get; set; }

    [Required]
    [StringLength(100)]
    public required string EnderecoCliente { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [ForeignKey("IdUsuario")]
    public required virtual Usuario Usuario { get; set; }

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<ClienteLivroEmprestimo> ClienteLivroEmprestimos { get; set; } = new List<ClienteLivroEmprestimo>();
}