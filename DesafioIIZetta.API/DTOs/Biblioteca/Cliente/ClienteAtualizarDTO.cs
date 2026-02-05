using System.ComponentModel.DataAnnotations;

public class ClienteAtualizarDTO{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
    public required string Nome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter entre 11 e 14 caracteres.")]
    public required string Cpf { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public required string Telefone { get; set; }

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public required string Endereco { get; set; }
}