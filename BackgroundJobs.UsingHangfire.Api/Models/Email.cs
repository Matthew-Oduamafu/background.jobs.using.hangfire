namespace BackgroundJobs.UsingHangfire.Api.Models;

public class Email
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
}