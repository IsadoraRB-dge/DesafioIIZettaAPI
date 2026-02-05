using DesafioIIZetta.API.Interfaces.Biblioteca;

namespace DesafioIIZetta.API.Services.Biblioteca;
// Isolando o cálculo aqui, podemos alterá-lo (ex: mudar o valor da multa) sem mexer no banco de dados.
public class MultaService : IMultaService{
    public decimal CalcularValorMulta(DateTime dataPrevista, DateTime? dataReal){
        // Se o livro ainda não foi devolvido (dataReal é nula),a multa é calculada com base na data atual.
        DateTime dataParaComparar = dataReal ?? DateTime.Now;
        // Apenas as datas são comparadas (ignorando as horas) para garantir um cálculo justo por dia.
        if (dataParaComparar.Date > dataPrevista.Date){
            var diasAtraso = (dataParaComparar.Date - dataPrevista.Date).Days;
            decimal valorDiario = 2.00m;
            return diasAtraso * valorDiario;
        }
        // Retorna zero se a devolução estiver dentro do prazo ou adiantada.
        return 0.00m;
    }
}