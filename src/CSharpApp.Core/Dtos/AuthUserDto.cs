using System.ComponentModel.DataAnnotations;

namespace CSharpApp.Core.Dtos;

public sealed class AuthUser
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    public string? PassWord { get; set; }
}