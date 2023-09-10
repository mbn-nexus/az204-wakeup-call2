using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace MarcBraun.Functions
{
    public class TimerTriggerMBN
    {
        public static string SendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey");

        [FunctionName("TimerTriggerMBN")]
        // TimerTrigger("* * * * * *")] // every second
        // TimerTrigger("s m h d m dow") // 0-59 0-59 0-23 1-31 1-12 0-6 (SUN-SAT)
        public async Task Run([TimerTrigger("0 0 8 * * 1")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (string.IsNullOrEmpty(SendGridApiKey))
            {
                log.LogError("SendGridApiKey is null");
                return;
            }

            var client = new SendGridClient(SendGridApiKey);
            var from = new EmailAddress("marc.braun@nexus-ag.de", "Marc Braun");
            var to = new EmailAddress("marc.braun@nexus-ag.de", "Marc Braun");
            var subject = "Good morning from Marcs Azure Function";
            var plainTextContent = "Wie gewünscht eine nette Nachricht am Montag morgen.";
            var htmlContent = "<strong>Wie gewünscht eine nette Nachricht am Montag morgen.</strong>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                log.LogInformation($"Email sent successfully at: {DateTime.Now}");
            else
                log.LogError($"Email not sent successfully at: {DateTime.Now}");
        }
    }
}
