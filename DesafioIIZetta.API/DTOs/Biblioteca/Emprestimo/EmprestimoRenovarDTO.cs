using System.ComponentModel.DataAnnotations;

namespace DesafioIIZetta.API.DTOs.Biblioteca.Emprestimo;

public class EmprestimoRenovarDTO{

    [Required(ErrorMessage = "A quantidade de dias adicionais é obrigatória.")]
    [Range(1, 30, ErrorMessage = "Você deve renovar para o cliente no mínimo de 1 dia e no máximo de 30 dias.")]
    public int DiasAdicionais { get; set; }
}