namespace DesafioIIZetta.API.Interfaces.Biblioteca;

public interface IMultaService{
    decimal CalcularValorMulta(DateTime dataPrevista, DateTime? dataReal);
}