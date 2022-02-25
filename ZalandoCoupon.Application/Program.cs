using System.Text.RegularExpressions;
using ZalandoCoupon.Application.Clients;

const int maxTries = 15;

var couponRegex = new Regex(">(NL[A-Z\\d]{8})<", RegexOptions.Compiled);

var mailClient = new MailClient();
var zalandoClient = new ZalandoClient();

Console.WriteLine("[1] Fetching email address");
var address = await mailClient.GetRandomEmailAddressAsync();

Console.WriteLine("[2] Subscribing to zalando newsletter");
await zalandoClient.RegisterToNewsletterAsync(address);

Console.Write($"\r[3] Retrieving mails {GetAttempts(0)}");

for (var i = 0; i < maxTries; i++)
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    
    var mails = await mailClient.GetMailsAsync(address);

    Console.Write($"\r[3] Retrieving mails {GetAttempts(i+1)}");
    
    foreach (var mail in mails)
    {
        var message = await mailClient.GetMessageAsync(address, mail.Id);

        var match = couponRegex.Match(message.HtmlBody);

        if (match.Success)
        {
            Console.WriteLine($"\r[3] Retrieving mails {GetAttempts(maxTries)}");

            Console.Write($"[4] Coupon: ");
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(match.Groups[1]);
            Console.ForegroundColor = defaultColor;
            Console.WriteLine();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            return;
        }
    }
}

Console.WriteLine("[4] No coupon found");
Console.WriteLine();
Console.WriteLine("Press enter to exit");

string GetAttempts(int current)
{
    return $"[{new string('#',current)}{new string('.',maxTries-current)}]";
}