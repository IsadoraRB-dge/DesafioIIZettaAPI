using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Mappings;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ControleEmprestimoLivroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddOpenApi();

builder.Services.AddScoped<IClienteRepo, ClienteRepository>();
builder.Services.AddScoped<ILivroRepo, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepo, EmprestimoRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment()){
    app.MapOpenApi();
}

app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
