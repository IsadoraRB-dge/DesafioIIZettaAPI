using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DesafioIIZetta.API.Models;

public partial class ControleEmprestimoLivroContext : DbContext{
    public ControleEmprestimoLivroContext(DbContextOptions<ControleEmprestimoLivroContext> options):base(options){
    }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<ClienteLivroEmprestimo> ClienteLivroEmprestimos { get; set; }
    public virtual DbSet<Livro> Livros { get; set; }

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


        modelBuilder.Entity<Livro>(entity =>{
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}