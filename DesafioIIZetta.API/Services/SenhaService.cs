using DesafioIIZetta.API.Interfaces.GestaoTarefas;

namespace DesafioIIZetta.API.Services;

// O isolamento da lógica de criptografia em um serviço especializado segue o 
// princípio da inversão de dependência, facilitando a troca da biblioteca de Hash se necessário.
public class SenhaService : ISenhaService{

    // Transforma a senha em texto plano em um Hash seguro utilizando BCrypt.
    public string GerarHash(string senha){
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool VerificarSenha(string senhaDigitada, string senhaDoBanco){
        return BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaDoBanco);
    }
}