using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BBSg
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new SendGridClient(@"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");
            var from = new EmailAddress("banderge@hotmail.com", "Speedy Gonzalez");
            var cc = new EmailAddress("aleksejs.piscevs@28stone.com");
            var bcc = new EmailAddress("jevgenijs.saveljevs@mizuhogroup.com");
            var to = new EmailAddress("jevgenijs.saveljevs@28stone.com");


            var message = GetTemplatedMessage(to, from);
            //var message = GetInlineMessage(to, from, "singleEmail");

            message.AddBcc(cc);
            message.AddCc(bcc);

            var byteData = Encoding.ASCII.GetBytes("bytes go here");
            message.Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Content = Convert.ToBase64String(byteData),
                        Filename = "Transcript.txt",
                        Type = "txt/plain",
                        Disposition = "attachment"
                    }
                };

            var response = await client.SendEmailAsync(message);
            Console.WriteLine(response.IsSuccessStatusCode);

            if (response.IsSuccessStatusCode is false)
            {
                var resp = await response.Body.ReadAsStringAsync();
                Console.WriteLine(resp);

            }

        }

        static SendGridMessage GetTemplatedMessage(EmailAddress to, EmailAddress from)
        {
            return MailHelper.CreateSingleTemplateEmail(from, to, "d-d0570494a0d74948a5f1c3c757d9ccd8", new
            {
                subject = "howdy",
                variable = "This is text from variable",
                Sender_Name = "Jevgenijs",
                render = false
            });
        }

        static SendGridMessage GetInlineMessage(EmailAddress to, EmailAddress from, string subject)
        {
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong> aaa";

            return MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        }
    }
}
