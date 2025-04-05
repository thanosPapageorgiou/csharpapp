using System.ComponentModel.DataAnnotations;

namespace CSharpApp.Core.Dtos;

public sealed class CreateProductRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = [];

    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }
}