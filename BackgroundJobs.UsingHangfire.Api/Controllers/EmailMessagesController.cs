using BackgroundJobs.UsingHangfire.Api.Contracts;
using BackgroundJobs.UsingHangfire.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJobs.UsingHangfire.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmailMessagesController : ControllerBase
{
    private readonly IRepository<EmailMessage> _repository;

    public EmailMessagesController(IRepository<EmailMessage> repository)
    {
        _repository = repository;
    }

    [HttpGet(Name = "GetAllMailMessages")]
    public async Task<IActionResult> GetAllMailMessages()
    {
        return Ok(await _repository.GetAll());
    }

    [HttpPost]
    public async Task<IActionResult> AddMailMessage(EmailMessage emailMessage)
    {
        var result = await _repository.Add(emailMessage, nameof(emailMessage.Id));
        return CreatedAtRoute("GetAllMailMessages", new { }, result);
    }
}