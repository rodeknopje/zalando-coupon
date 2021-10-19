using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zalando.Clients;

namespace Zalando
{
    public class Program
    {
        static async Task Main()
        {
            var guerrillaClient = new guerrillaClient();
            var zalandoClient = new ZalandoClient();
            
            Console.WriteLine("Creating email address...");
            var email = await guerrillaClient.GetEmailAddressAsync();

            Console.WriteLine("Signing up for newsletter... ");
            await zalandoClient.RegisterToNewsletterAsync(email.EmailAddr);

            var zalandoMail = await guerrillaClient.WaitForMessageFromAsync(email.SidToken, "zalando");
            Console.WriteLine();
            if (zalandoMail == null)
            {
                Console.WriteLine("error");

                Console.WriteLine("Press enter to exit...");

                Console.ReadLine();

                return;
            }
            var message = await guerrillaClient.FetchEmailAsync(email.SidToken, zalandoMail.MailId.ToString());

            var couponRegex = new Regex(">(NL[A-Z\\d]{8})<");

            var coupon = couponRegex.Match(message.MailBody).Groups[1].ToString();

            Console.Write("Coupon: ");
            Console.WriteLine(coupon);

            Console.WriteLine("Press enter to exit...");

            Console.ReadLine();
        }
    }
}