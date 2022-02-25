namespace ZalandoCoupon.Application.Clients;

public class ZalandoClient
{
    private readonly HttpClient _client;
    private const string BaseAddress = "https://www.zalando.nl/api/";
    
    public ZalandoClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(BaseAddress);
    }

    public async Task RegisterToNewsletterAsync(string email)
    {
        var content = $"[{{\"id\":\"f321f59294a4ffd369951dc5d8f92b801cb7c3c7302de9e5118b3569416c844f\",\"variables\":{{\"input\":{{\"email\":\"{email}\",\"preference\":{{\"category\":\"MEN\",\"topics\":[{{\"id\":\"item_alerts\",\"isEnabled\":true}},{{\"id\":\"recommendations\",\"isEnabled\":true}},{{\"id\":\"fashion_fix\",\"isEnabled\":true}},{{\"id\":\"follow_brand\",\"isEnabled\":true}},{{\"id\":\"subscription_confirmations\",\"isEnabled\":true}},{{\"id\":\"survey\",\"isEnabled\":true}},{{\"id\":\"offers_sales\",\"isEnabled\":true}}]}},\"referrer\":\"nl_subscription_page\",\"clientMutationId\":\"1630664083241\"}}}}}}]";
            
        var response = await _client.PostAsync("graphql", new StringContent(content));

        response.EnsureSuccessStatusCode();
    }

}