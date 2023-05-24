using BackgroundJobs.UsingHangfire.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs.UsingHangfire.Api.Data;

public class HangfireDbContext : DbContext
{
    public HangfireDbContext(DbContextOptions<HangfireDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<EmailMessage> EmailMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "Matthew Oduamafu",
                PhoneNumber = "0552474843",
                EmailAddress = "mattmadjitey@gmail.com"
            },
            new User
            {
                Id = 2,
                Name = "Matt Oduamafu",
                PhoneNumber = "0552235521",
                EmailAddress = "mattoduamafu@hotmail.com"
            }
        );

        modelBuilder.Entity<EmailMessage>().HasData(
            new EmailMessage()
            {
                Id = 1,
                Subject = "Your application results",
                Body = @"Hello!
Thank you for applying to the Senior Software Engineer (.Net Full Stack and Azure/AWS) position at EPAM India. You have an impressive resume, and it was a pleasure to learn more about your skills and accomplishments.

However, we regret to inform you that we have decided to not pursue your candidature currently due to the location demands of the role. But now that we have had the chance to know more about you, we would like to keep your resume on file for future openings that may better fit your profile and location.

In the meantime, feel free to apply to other suitable positions on our careers page. Thank you for considering EPAM to be a part of your career journey.

We wish you good luck in your job search and all future endeavours.

Regards,

Team EPAM India",
                HtmlContext = @"<!DOCTYPE html>
                            <html>
                            <head>
                                <meta charset='UTF-8'>
                                <title>Email Message</title>
                            </head>
                            <body>
                                <p>Hello!</p>
                                <p>Thank you for applying to the Senior Software Engineer (.Net Full Stack and Azure/AWS) position at EPAM India. You have an impressive resume, and it was a pleasure to learn more about your skills and accomplishments.</p>
                                <p>However, we regret to inform you that we have decided to not pursue your candidature currently due to the location demands of the role. But now that we have had the chance to know more about you, we would like to keep your resume on file for future openings that may better fit your profile and location.</p>
                                <p>In the meantime, feel free to apply to other suitable positions on our careers page. Thank you for considering EPAM to be a part of your career journey.</p>
                                <p>We wish you good luck in your job search and all future endeavors.</p>
                                <p>Regards,</p>
                                <p>Team EPAM India</p>
                            </body>
                            </html>"
            }
        );

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Users");
        });
        modelBuilder.Entity<EmailMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("EmailMessages");
        });
    }
}