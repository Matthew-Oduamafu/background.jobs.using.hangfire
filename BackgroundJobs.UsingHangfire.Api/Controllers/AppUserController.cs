using System.Net.Mail;
using BackgroundJobs.UsingHangfire.Api.Contracts;
using BackgroundJobs.UsingHangfire.Api.Models;
using FluentEmail.Smtp;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJobs.UsingHangfire.Api.Controllers;

[ApiController]
[Route("api/[Controller]/[action]")]
public class AppUserController : ControllerBase
{
    private readonly IRepository<User> _repository;
    private readonly IEmailSender _emailSender;

    public AppUserController(IRepository<User> repository, IEmailSender emailSender)
    {
        _repository = repository;
        _emailSender = emailSender;
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _repository.GetAll());
    }

    [HttpGet(Name = "GetById")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        return Ok(await _repository.Get(x => x.Id == id));
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        var result = await _repository.Add(user, nameof(user.Id));

        try
        {
            Email mail = new()
            {
                To = user.EmailAddress,
                Subject = EmailSubject,
                Body = EmailContent,
                HtmlContent = HtmlContent,
            };

            var jobId = BackgroundJob.Enqueue<IEmailSender>(x => x.SendSmtp(mail, user));

            Console.WriteLine("Message sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return CreatedAtRoute("GetById", new { id = result }, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        var result = await _repository.Update(user, nameof(user.Id));
        return AcceptedAtRoute("GetById", new { id = result }, result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery] int id)
    {
        await _repository.Delete(x => x.Id == id);
        return NoContent();
    }

    private const string EmailSubject = "Subscription to Our Services";

    private const string EmailContent = @"Dear [User's Name],

Thank you for your interest in subscribing to our services. We are excited to have you as a valued customer and look forward to providing you with top-notch service and support.

To complete your subscription, please follow the steps below:

1. Visit our website at [website URL].
2. Click on the ""Subscribe"" button or navigate to the subscription page.
3. Select the desired service package that best suits your needs.
4. Provide the necessary information, including your contact details and preferred payment method.
5. Review the terms and conditions, and if you agree, click on the ""Subscribe"" or ""Submit"" button.
6. You will receive a confirmation email shortly after successful subscription.

Once your subscription is confirmed, you will gain access to all the benefits and features of our services. Our team is dedicated to ensuring your satisfaction and will be available to assist you with any inquiries or support you may need along the way.

If you have any questions or require further assistance during the subscription process, please don't hesitate to reach out to our customer support team at [customer support contact details]. We are here to help!

Thank you again for choosing our services. We value your trust and are committed to delivering an exceptional experience.

Best regards,

[Your Name]
[Your Company]";

    private const string HtmlContent = @"<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Subscription to Our Services</title>
</head>
<body>
    <p>Dear @Model.Name,</p>
    <p>Thank you for your interest in subscribing to our services. We are excited to have you as a valued customer and look forward to providing you with top-notch service and support.</p>
    <p>To complete your subscription, please follow the steps below:</p>
    <ol>
        <li>Visit our website at [website URL].</li>
        <li>Click on the ""Subscribe"" button or navigate to the subscription page.</li>
        <li>Select the desired service package that best suits your needs.</li>
        <li>Provide the necessary information, including your contact details and preferred payment method.</li>
        <li>Review the terms and conditions, and if you agree, click on the ""Subscribe"" or ""Submit"" button.</li>
        <li>You will receive a confirmation email shortly after successful subscription.</li>
    </ol>
    <p>Once your subscription is confirmed, you will gain access to all the benefits and features of our services. Our team is dedicated to ensuring your satisfaction and will be available to assist you with any inquiries or support you may need along the way.</p>
    <p>If you have any questions or require further assistance during the subscription process, please don't hesitate to reach out to our customer support team at [customer support contact details]. We are here to help!</p>
    <p>Thank you again for choosing our services. We value your trust and are committed to delivering an exceptional experience.</p>
    <p>Best regards,</p>
    <p>[Your Name]</p>
    <p>[Your Company]</p>
</body>
</html>";
}