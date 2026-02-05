namespace DesafioIIZetta.API.Interfaces.GestaoTarefas;

public interface ISenhaService{
    string GerarHash(string senha);
    bool VerificarSenha(string senhaDigitada, string senhaDoBanco);
}