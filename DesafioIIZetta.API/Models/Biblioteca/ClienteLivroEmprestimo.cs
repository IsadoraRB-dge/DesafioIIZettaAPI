using DesafioIIZetta.API.Models.Biblioteca;
using DesafioIIZetta.API.Models.GestaoTarefas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioIIZetta.API.Models;

[Table("Cliente_Livro_Emprestimo")]
public partial class ClienteLivroEmprestimo{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public int IdLivro { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataEmprestimo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DataDevolucaoPrevista { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataDevolucaoReal { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("ClienteLivroEmprestimos")]
    public required virtual Cliente IdClienteNavigation { get; set; }

    [ForeignKey("IdLivro")]
    [InverseProperty("ClienteLivroEmprestimos")]
    public required virtual Livro IdLivroNavigation { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Emprestimos")]
    public required virtual Usuario UsuarioNavigation { get; set; }

}