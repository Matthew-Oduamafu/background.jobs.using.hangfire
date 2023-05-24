namespace BackgroundJobs.UsingHangfire.Api.Models;

public class EmailSettings
{
    public string FromAddress { get; set; } = null!;
    public string FromName { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}