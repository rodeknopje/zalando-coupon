using System.Text.RegularExpressions;
using mailslurp.Api;
using mailslurp.Client;
using ZalandoCoupon.Application.Clients;

var couponRegex = new Regex(">(NL[A-Z\\d]{8})<", RegexOptions.Compiled);

var configuration = new Configuration();

var mailSlurpApi = Environment.GetEnvironmentVariable("MailSlurpApi");

if (mailSlurpApi is null)
{
    Console.WriteLine("No MailSlurp api key provided");
    Console.ReadKey();
    return;
}

configuration.ApiKey.Add("x-api-key", mailSlurpApi);

var mailSlurp = new InboxControllerApi(configuration);

Console.WriteLine("[1] Creating inbox.");
var inbox = await mailSlurp.CreateInboxAsync(expiresAt: DateTime.Now.AddMinutes(3));

Console.WriteLine($"[3] Signing up for zalando newsletter.");
await new ZalandoClient().RegisterToNewsletterAsync(inbox.EmailAddress);

var waitForInstance = new WaitForControllerApi(configuration);

Console.WriteLine("[4] Waiting for email.");

var email = await waitForInstance.WaitForLatestEmailAsync(inbox.Id, TimeSpan.FromMinutes(2).Milliseconds, true) ?? throw new Exception("No email received");

var match = couponRegex.Match(email.Body);

if (match.Success)
{
    Console.Write($"[5] Coupon: ");
    var defaultColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(match.Groups[1]);
    Console.ForegroundColor = defaultColor;
    Console.WriteLine();
    Console.WriteLine("Press enter to exit");
    Console.ReadLine();
}