using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Repositories;
using Microsoft.EntityFrameworkCore;
using DesafioIIZetta.API.Interfaces;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ControleEmprestimoLivroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IClienteRepo, ClienteRepository>();
builder.Services.AddScoped<ILivroRepo, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepo, EmprestimoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
