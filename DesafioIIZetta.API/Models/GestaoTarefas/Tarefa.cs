#nullable disable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioIIZetta.API.Models.GestaoTarefas;

[Table("Tarefa")]
public partial class Tarefa{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdTarefa { get; set; }

    [Required]
    [StringLength(150)]
    public string NomeTarefa { get; set; }

    [Required]
    [StringLength(500)]
    public string DescricaoTarefa { get; set; }

    [Required]
    public string StatusTarefa { get; set; } 

    [Required]
    public int IdUsuario { get; set; }

    [Required]
    [StringLength(20)]
    public string Prioridade { get; set; }="Baixa";

    [ForeignKey("IdUsuario")]
    [InverseProperty("Tarefas")]
    public virtual Usuario IdUsuarioNavigation { get; set; }
}