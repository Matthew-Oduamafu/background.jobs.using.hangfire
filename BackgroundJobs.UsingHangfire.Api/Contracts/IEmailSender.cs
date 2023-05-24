using BackgroundJobs.UsingHangfire.Api.Models;

namespace BackgroundJobs.UsingHangfire.Api.Contracts;

public interface IEmailSender
{
    Task<bool> Send(Email email);
    Task<bool> SendBatchMail();
    Task<bool> SendBatchMailSmtp();
    Task<bool> SendSmtp(Email email, User user);
}