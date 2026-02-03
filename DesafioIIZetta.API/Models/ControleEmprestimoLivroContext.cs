using System;
using System.Collections.Generic;
using DesafioIIZetta.API.Models.Biblioteca;
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Models;

public partial class ControleEmprestimoLivroContext : DbContext{
    public ControleEmprestimoLivroContext(DbContextOptions<ControleEmprestimoLivroContext> options):base(options){
    }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<ClienteLivroEmprestimo> ClienteLivroEmprestimos { get; set; }
    public virtual DbSet<Livro> Livros { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
 
        modelBuilder.Entity<Cliente>(entity =>{
        });

        modelBuilder.Entity<ClienteLivroEmprestimo>(entity =>{
          
            entity.HasOne(d => d.IdClienteNavigation)
                .WithMany(p => p.ClienteLivroEmprestimos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Livro_Emprestimo_Cliente");

            entity.HasOne(d => d.IdLivroNavigation)
                .WithMany(p => p.ClienteLivroEmprestimos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Livro_Emprestimo_Livro");
        });


        modelBuilder.Entity<Livro>(entity =>{});

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Tarefa_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}