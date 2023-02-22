using System.Text.Json.Serialization;

namespace ApiCatalogoMinimals.Models;

public class Produto
{
    public int ProdutoId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public double Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public DateTime DataCompra { get; set; }
    public int Estoque { get; set; }


    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }
}
