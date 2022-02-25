using System.Text.Json.Serialization;

namespace ZalandoCoupon.Application.Models;

[Serializable]
public class Message
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("from")] public string From { get; set; } = default!;
    [JsonPropertyName("subject")] public string Subject { get; set; } = default!;
    [JsonPropertyName("date")] public string Date { get; set; } = default!;
    [JsonPropertyName("body")] public string Body { get; set; } = default!;
    [JsonPropertyName("textBody")] public string TextBody { get; set; } = default!;
    [JsonPropertyName("htmlBody")] public string HtmlBody { get; set; } = default!;
}