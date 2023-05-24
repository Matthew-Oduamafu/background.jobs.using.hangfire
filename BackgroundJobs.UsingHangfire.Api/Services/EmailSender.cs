using System.Net;
using System.Net.Mail;
using System.Text;
using BackgroundJobs.UsingHangfire.Api.Contracts;
using BackgroundJobs.UsingHangfire.Api.Models;
using FluentEmail.Core.Defaults;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BackgroundJobs.UsingHangfire.Api.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly IRepository<User> _repository;
    private readonly IRepository<EmailMessage> _mailRepository;

    public EmailSender(IOptions<EmailSettings> options, IRepository<User> repository,
        IRepository<EmailMessage> mailRepository)
    {
        _repository = repository;
        _mailRepository = mailRepository;
        _emailSettings = options.Value;
    }

    public async Task<bool> Send(Email email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var to = new EmailAddress { Email = email.To };
        var from = new EmailAddress { Name = _emailSettings.FromName, Email = _emailSettings.FromAddress };

        var message = MailHelper.CreateSingleEmail(
            from,
            to,
            email.Subject,
            email.Subject,
            email.HtmlContent);

        var response = await client.SendEmailAsync(message);

        return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;
    }

    public async Task<bool> SendBatchMail()
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var tos = (await _repository.GetAll())
            .Select(x => new EmailAddress { Email = x.EmailAddress, Name = x.Name }).ToList();
        var mailMessage = (await _mailRepository.GetAll()).MinBy(x => x.Id);

        var from = new EmailAddress { Name = _emailSettings.FromName, Email = _emailSettings.FromAddress };

        var message = MailHelper.CreateSingleEmailToMultipleRecipients(
            from,
            tos,
            mailMessage?.Subject,
            mailMessage?.Body,
            mailMessage?.HtmlContext);

        var response = await client.SendEmailAsync(message);

        return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;
    }

    public async Task<bool> SendBatchMailSmtp()
    {
        var sender = new SmtpSender(() => new SmtpClient()
        {
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 25,
            Host = "localhost",
        });

        var emailBody = @"Hello!
Thank you for applying to the Senior Software Engineer (.Net Full Stack and Azure/AWS) position at EPAM India. You have an impressive resume, and it was a pleasure to learn more about your skills and accomplishments.

However, we regret to inform you that we have decided to not pursue your candidature currently due to the location demands of the role. But now that we have had the chance to know more about you, we would like to keep your resume on file for future openings that may better fit your profile and location.

In the meantime, feel free to apply to other suitable positions on our careers page. Thank you for considering EPAM to be a part of your career journey.

We wish you good luck in your job search and all future endeavours.

Regards,

Team EPAM India";

        // using template 
        StringBuilder builder = new();
        builder.AppendLine("Dear @Model.FirstName")
            .AppendLine("<p>Thank you for purchasing <em> @Model.ProductName</em>. We hope you enjoy it.</p>")
            .AppendLine("<strong><em>- The TimCo Team</em></strong>");

        FluentEmail.Core.Email.DefaultSender = sender;
        FluentEmail.Core.Email.DefaultRenderer = new RazorRenderer(); // do this when using email templates


        var email = await FluentEmail.Core.Email
            .From("mattoduamafu@gmail.com")
            .To("mattmadjitey@gmail.com")
            .Subject("Subscription to Our Services")
            .UsingTemplate(builder.ToString(), new { FirstName = "Matthew", ProductName = "This way chocolate drink" })
            // .Body(emailBody)
            .SendAsync();

        return email.Successful;
    }

    public async Task<bool> SendSmtp(Email email, User user)
    {
        var emailSender = new SmtpSender(() => new SmtpClient()
        {
            Host = "localhost",
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Port = 25,
        });

        FluentEmail.Core.Email.DefaultSender = emailSender;
        FluentEmail.Core.Email.DefaultRenderer = new ReplaceRenderer();
        StringBuilder builder = new();
        builder.AppendLine(email.HtmlContent);

        var emailResponse = await FluentEmail.Core.Email
            .From("mattietorrent@gmail.com")
            .To(email.To)
            .Subject(email.Subject)
            .UsingTemplate(builder.ToString(), user)
            .SendAsync();

        return emailResponse.Successful;
    }
}