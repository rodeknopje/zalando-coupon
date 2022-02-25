using System.Text.Json.Serialization;

namespace ZalandoCoupon.Application.Models;

[Serializable]
public class Mail
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("from")] public string From { get; set; } = default!;
    [JsonPropertyName("subject")] public string Subject { get; set; } = default!;
    [JsonPropertyName("date")] public string Date { get; set; } = default!;
}