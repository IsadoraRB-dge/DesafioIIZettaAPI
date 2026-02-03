#nullable disable
using System;
using System.Collections.Generic;
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
    public string NomeUsuario { get; set; }

    [Required]
    [StringLength(100)]
    public string EmailUsuario { get; set; }

    [Required]
    [StringLength(255)] 
    public string SenhaUsuario { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}