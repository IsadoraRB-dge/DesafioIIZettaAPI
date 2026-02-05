using DesafioIIZetta.API.Services.Biblioteca;

namespace DesafioIIZetta.Tests;

public class MultaServiceTests{
    private readonly MultaService _multaService;

    public MultaServiceTests(){
     
        _multaService = new MultaService();
    }

    [Fact] 
    public void CalcularValorMulta_DeveRetornarQuatroReais_QuandoAtrasoForDeDoisDias(){
     
        var dataPrevista = new DateTime(2026, 02, 01);
        var dataReal = new DateTime(2026, 02, 03); 

      
        var resultado = _multaService.CalcularValorMulta(dataPrevista, dataReal);

    
        Assert.Equal(4.00m, resultado);
    }
}