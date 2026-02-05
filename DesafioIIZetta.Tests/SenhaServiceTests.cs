using DesafioIIZetta.API.Services;

namespace DesafioIIZetta.Tests;

public class SenhaServiceTests{
    private readonly SenhaService _senhaService;

    public SenhaServiceTests(){
        _senhaService = new SenhaService();
    }

    [Fact]
    public void GerarHash_DeveCriarSenhaCriptografada_DiferenteDoTextoPuro(){
       
        string senhaPura = "Comandante123";

        string hashGerado = _senhaService.GerarHash(senhaPura);

 
        Assert.NotEqual(senhaPura, hashGerado); 
        Assert.True(hashGerado.Length > 20);   
    }

    [Fact]
    public void VerificarSenha_DeveRetornarTrue_QuandoSenhaForCorreta(){

        string senhaCorreta = "MinhaSenhaSegura";
        string hashNoBanco = _senhaService.GerarHash(senhaCorreta);

        bool resultado = _senhaService.VerificarSenha(senhaCorreta, hashNoBanco);


        Assert.True(resultado); 
    }
}