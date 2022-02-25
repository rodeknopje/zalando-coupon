using System.Text.Json;
using ZalandoCoupon.Application.Models;

namespace ZalandoCoupon.Application.Clients;

public class MailClient
{
    private readonly HttpClient _client;
    private const string BaseAddress = "https://1secmail.com/api/v1/";

    public MailClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(BaseAddress);
    }

    public async Task<string> GetRandomEmailAddressAsync()
    {
        var response = await _client.GetAsync("?action=genRandomMailbox&count=1");

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        var list = JsonSerializer.Deserialize<List<string>>(body);

        if (list is null || list.Any() == false)
        {
            throw new NullReferenceException(nameof(list));
        }

        return list.FirstOrDefault() ?? string.Empty;
    }

    public async Task<List<Mail>> GetMailsAsync(string address)
    {
        var addressParts = address.Split('@');

        var response = await _client.GetAsync($"?action=getMessages&login={addressParts[0]}&domain={addressParts[1]}");

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        var mails = JsonSerializer.Deserialize<List<Mail>>(body);

        if (mails is null)
        {
            throw new NullReferenceException(nameof(mails));
        }

        return mails;
    }

    public async Task<Message> GetMessageAsync(string address, int mailId)
    {
        var addressParts = address.Split('@');

        var response = await _client.GetAsync($"?action=readMessage&login={addressParts[0]}&domain={addressParts[1]}&id={mailId}");

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        var messages = JsonSerializer.Deserialize<Message>(body);

        if (messages is null)
        {
            throw new NullReferenceException(nameof(messages));
        }

        return messages;
    }
}