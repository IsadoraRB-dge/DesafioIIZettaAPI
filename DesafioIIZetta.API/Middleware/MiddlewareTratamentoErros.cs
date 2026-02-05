using System.Net;
using System.Text.Json;
using DesafioIIZetta.API.Excecoes;

namespace DesafioIIZetta.API.Middleware;

public class RespostaErro
{
    public bool Sucesso => false;
    public string Modulo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public void Configurar(string mod, string msg) { Modulo = mod; Mensagem = msg; }
}

public class MiddlewareTratamentoErros{
    private readonly RequestDelegate _next;
    private readonly ILogger<MiddlewareTratamentoErros> _logger;

    public MiddlewareTratamentoErros(RequestDelegate next, ILogger<MiddlewareTratamentoErros> logger){
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context){
        try {
            // Tenta seguir com a requisição para o próximo componente do pipeline (Controller)
            await _next(context); 
        }
        catch (Exception ex) {
            // Captura qualquer erro não tratado na aplicação de forma centralizada
            await TratarExcecaoAsync(context, ex); 
        }
    }

    private Task TratarExcecaoAsync(HttpContext context, Exception excecao){
        var resposta = new RespostaErro();
        context.Response.ContentType = "application/json";
        // Verifica se a exceção é uma regra de negócio conhecida 
        if (excecao is ExcecaoBaseProjeto exBase)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            // Identifica dinamicamente qual módulo disparou o erro para facilitar o debug no Front-end
            var moduloNome = exBase.GetType().Name.Replace("Exception", "");

            if (moduloNome == "Tarefa") moduloNome = "Gestão de Tarefas";

                resposta.Configurar(moduloNome, exBase.Message);
            }
            else{
                // Erros desconhecidos são logados no servidor, mas o cliente recebe uma mensagem genérica por segurança
                _logger.LogError(excecao, "Erro crítico no servidor");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                resposta.Configurar("Sistema", "Ocorreu um erro interno inesperado.");
        }

        var opcoesJson = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(resposta, opcoesJson));
    }
}

