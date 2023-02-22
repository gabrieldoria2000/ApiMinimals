using ApiCatalogoMinimals.Context;
using ApiCatalogoMinimals.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoMinimals.ApiEndpoints;

public static class CategoriasEndpoints
{

    public static void MapCategoriasEndpoints(this WebApplication app)
    {
        app.MapGet("/Categorias", async (AppDbContexxt db) => await db.Categorias.ToListAsync())
    .WithTags("Categorias").RequireAuthorization();

        app.MapGet("/Categorias/{id:int}", async (int id, AppDbContexxt db) =>
        {
            return await db.Categorias.FindAsync(id)
                is Categoria categoria
                    ? Results.Ok(categoria)
                    : Results.NotFound();
        });

        app.MapPost("/Categorias", async (Categoria categoria, AppDbContexxt db)
            =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();

            //abaixo só para exibir a categoria que foi criada!
            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        }
                );

        app.MapPut("/Categorias/{id:int}", async (int id, Categoria categoria, AppDbContexxt db) =>
        {
            if (categoria.CategoriaId != id)
            {
                return Results.BadRequest();
            }

            var CategoriaBD = await db.Categorias.FindAsync(id);

            if (CategoriaBD is null) return Results.NotFound();

            CategoriaBD.Nome = categoria.Nome;
            CategoriaBD.Descricao = categoria.Descricao;

            await db.SaveChangesAsync();

            return Results.Ok(CategoriaBD);

        }
                );

        app.MapDelete("/Categorias/{id:int}", async (int id, AppDbContexxt db) =>
        {
            var CategoriaBD = await db.Categorias.FindAsync(id);

            if (CategoriaBD is null) return Results.NotFound();

            db.Categorias.Remove(CategoriaBD);

            await db.SaveChangesAsync();

            return Results.NoContent();

        });
    }
}
