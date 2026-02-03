#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Models.Biblioteca;

[Table("Cliente")]
public partial class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCliente { get; set; }

    [Required]
    [StringLength(150)]
    public string NomeCliente { get; set; }

    [Required]
    [Column("CPFCliente")]
    [StringLength(14)]
    public string Cpfcliente { get; set; }

    [Required]
    [StringLength(20)]
    public string TelefoneCliente { get; set; }

    [Required]
    [StringLength(100)]
    public string EmailCliente { get; set; }

    [Required]
    [StringLength(100)]
    public string EnderecoCliente { get; set; }

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<ClienteLivroEmprestimo> ClienteLivroEmprestimos { get; set; } = new List<ClienteLivroEmprestimo>();
}