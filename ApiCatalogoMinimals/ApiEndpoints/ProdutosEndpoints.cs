using ApiCatalogoMinimals.Context;
using ApiCatalogoMinimals.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimals.ApiEndpoints;

public static class ProdutosEndpoints
{

    public static void MapProdutosEndpoints(this WebApplication app)
    {
        //------------------------endpoints para Produto ---------------------------------
        app.MapPost("/produtos", async (Produto produto, AppDbContexxt db)
         => {
             db.Produtos.Add(produto);
             await db.SaveChangesAsync();

             return Results.Created($"/produtos/{produto.ProdutoId}", produto);
         });

        app.MapGet("/produtos", async (AppDbContexxt db) => await db.Produtos.ToListAsync());

        app.MapGet("/produtos/{id:int}", async (int id, AppDbContexxt db)
            => {
                return await db.Produtos.FindAsync(id)
                             is Produto produto
                             ? Results.Ok(produto)
                             : Results.NotFound();
            });

        app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContexxt db) =>
        {

            if (produto.ProdutoId != id)
            {
                return Results.BadRequest();
            }

            var produtoDB = await db.Produtos.FindAsync(id);

            if (produtoDB is null) return Results.NotFound();

            produtoDB.Nome = produto.Nome;
            produtoDB.Descricao = produto.Descricao;
            produtoDB.Preco = produto.Preco;
            produtoDB.ImagemUrl = produto.ImagemUrl;
            produtoDB.DataCompra = produto.DataCompra;
            produtoDB.Estoque = produto.Estoque;
            produtoDB.CategoriaId = produto.CategoriaId;

            await db.SaveChangesAsync();

            return Results.Ok(produtoDB);
        });

        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContexxt db) =>
        {
            var produto = await db.Produtos.FindAsync(id);

            if (produto is null)
            {
                return Results.NotFound();
            }

            db.Produtos.Remove(produto);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}