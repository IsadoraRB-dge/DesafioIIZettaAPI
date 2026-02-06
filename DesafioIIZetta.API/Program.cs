using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Interfaces.Biblioteca;
using DesafioIIZetta.API.Interfaces.GestaoTarefas;
using DesafioIIZetta.API.Mappings;
using DesafioIIZetta.API.Middleware;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Repositories;
using DesafioIIZetta.API.Repositories.Biblioteca;
using DesafioIIZetta.API.Services;
using DesafioIIZetta.API.Services.Biblioteca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Banco de Dados ---
builder.Services.AddDbContext<ControleEmprestimoLivroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. Autenticação JWT ---
var chaveString = builder.Configuration["Jwt:ChaveSecreta"] ?? "ChavePadraoParaDesenvolvimento123!";
var chaveBytes = Encoding.ASCII.GetBytes(chaveString);

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

// --- 3. Controllers e JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });


builder.Services.Configure<ApiBehaviorOptions>(options =>{
    options.InvalidModelStateResponseFactory = context =>{
        var mensagemErro = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "Dados de entrada inválidos.";

        var resposta = new RespostaErro();
        resposta.Configurar("Validação", mensagemErro);

        return new BadRequestObjectResult(resposta);
    };
});

// --- 4. Injeção de Dependência ---
builder.Services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IClienteRepo, ClienteRepository>();
builder.Services.AddScoped<ILivroRepo, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepo, EmprestimoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IMultaService, MultaService>();
builder.Services.AddScoped<ISenhaService, SenhaService>();

// --- 5. AutoMapper ---
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();


// --- 6. Configuração de Cultura ---
var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// --- 7. Pipeline de Execução ---
app.UseMiddleware<MiddlewareTratamentoErros>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ControleEmprestimoLivroContext>(); 
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar banco: {ex.Message}");
    }
}

app.Run();