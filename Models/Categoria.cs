using System.Text.Json.Serialization;

namespace ApiCatalogo.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }

        
        [JsonIgnore] // Não exibir essa propriedade no Post, pois este é um atributo de navegação.
        public ICollection<Produto>? Lista_Produtos { get; set; }
    }
}
