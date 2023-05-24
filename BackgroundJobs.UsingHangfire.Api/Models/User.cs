
namespace BackgroundJobs.UsingHangfire.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }=null!;
    public string PhoneNumber { get; set; }=null!;
    public string EmailAddress { get; set; } = null!;
}