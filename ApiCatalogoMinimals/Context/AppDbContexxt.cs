using ApiCatalogoMinimals.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimals.Context;

public class AppDbContexxt : DbContext
{
    public AppDbContexxt(DbContextOptions<AppDbContexxt> options) : base(options)
    {

    }

    public DbSet<Produto>?  Produtos { get; set; }
    public DbSet<Categoria>? Categorias { get; set; }

    protected  override void OnModelCreating(ModelBuilder mb)
    {
        //Esse método vai ser chamado na primeira vez que o framework for executar os mapeamentos no banco e memoria
        //vamos definir os relacionamentos de forma explicita usando a FLUENT API


        //CATEGORIA
        mb.Entity<Categoria>().HasKey(c => c.CategoriaId);

        mb.Entity<Categoria>().Property(c => c.Nome).HasMaxLength(100).IsRequired();

        mb.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();

        //PRODUTO
        mb.Entity<Produto>().HasKey(c => c.ProdutoId);

        mb.Entity<Produto>().Property(c => c.Nome).HasMaxLength(100).IsRequired();
        mb.Entity<Produto>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();
        mb.Entity<Produto>().Property(c => c.ImagemUrl).HasMaxLength(100).IsRequired();

        mb.Entity<Produto>().Property(c => c.Preco).HasPrecision(14, 2);

        //RELACIONAMENTO
        mb.Entity<Produto>().HasOne<Categoria>(c => c.Categoria)
            .WithMany(p => p.Produtos)
            .HasForeignKey(c => c.CategoriaId);
    }

}
