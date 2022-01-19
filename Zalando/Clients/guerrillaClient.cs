using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Zalando.Models.Guerilla;
using Zalando.Models.Guerrilla;

namespace Zalando.Clients;

public class guerrillaClient
{
    private HttpClient Client { get; set; }

    private readonly JsonSerializerSettings _jsonSettings;

    public guerrillaClient()
    {
        Client = new HttpClient {BaseAddress = new Uri("https://api.guerrillamail.com/ajax.php")};
            
        _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ContractResolver = new DefaultContractResolver {NamingStrategy = new SnakeCaseNamingStrategy()}
        };
    }

    public async Task<Email> GetEmailAddressAsync()
    {
        const string url = "?f=get_email_address";

        using var response = await Client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var email = JsonConvert.DeserializeObject<Email>(content, _jsonSettings);

        // initialize mailbox.
        await Client.GetAsync($"?f=get_email_list&sid_token={email?.SidToken}&offset=0");

        return email;
    }

    public async Task<Inbox> GetEmailInboxAsync(string sidToken)
    {
        var url = $"?f=check_email&sid_token={sidToken}&seq=0";

        using var response = await Client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var inbox = JsonConvert.DeserializeObject<Inbox>(content, _jsonSettings);

        return inbox;
    }


    public async Task<FetchEmail> FetchEmailAsync(string sidToken, string emailId)
    {
        var url = $"?f=fetch_email&sid_token={sidToken}&email_id={emailId}";

        using var response = await Client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var inbox = JsonConvert.DeserializeObject<FetchEmail>(content, _jsonSettings);

        return inbox;
    }

    public async Task<Message> WaitForMessageFromAsync(string sidToken, string sender)
    {
        const int maxTries = 20;
            
        for (var i = 0; i < maxTries; i++)
        {
            await Task.Delay(5000);

            Console.Write($"\rChecking email... [{i+1}/{maxTries}]  ");

            var inbox = await GetEmailInboxAsync(sidToken);
                
            var zalandoMail = inbox.List.FirstOrDefault(x => x.MailFrom.Contains(sender));

            if (zalandoMail != null)
            {
                return zalandoMail;
            }
        }
            
        return null;
    }
}