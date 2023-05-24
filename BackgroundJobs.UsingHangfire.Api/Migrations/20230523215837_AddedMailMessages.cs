using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackgroundJobs.UsingHangfire.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedMailMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HtmlContext = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EmailMessages",
                columns: new[] { "Id", "Body", "HtmlContext", "Subject" },
                values: new object[] { 1, "Hello!\r\nThank you for applying to the Senior Software Engineer (.Net Full Stack and Azure/AWS) position at EPAM India. You have an impressive resume, and it was a pleasure to learn more about your skills and accomplishments.\r\n\r\nHowever, we regret to inform you that we have decided to not pursue your candidature currently due to the location demands of the role. But now that we have had the chance to know more about you, we would like to keep your resume on file for future openings that may better fit your profile and location.\r\n\r\nIn the meantime, feel free to apply to other suitable positions on our careers page. Thank you for considering EPAM to be a part of your career journey.\r\n\r\nWe wish you good luck in your job search and all future endeavours.\r\n\r\nRegards,\r\n\r\nTeam EPAM India", "<!DOCTYPE html>\r\n                            <html>\r\n                            <head>\r\n                                <meta charset='UTF-8'>\r\n                                <title>Email Message</title>\r\n                            </head>\r\n                            <body>\r\n                                <p>Hello!</p>\r\n                                <p>Thank you for applying to the Senior Software Engineer (.Net Full Stack and Azure/AWS) position at EPAM India. You have an impressive resume, and it was a pleasure to learn more about your skills and accomplishments.</p>\r\n                                <p>However, we regret to inform you that we have decided to not pursue your candidature currently due to the location demands of the role. But now that we have had the chance to know more about you, we would like to keep your resume on file for future openings that may better fit your profile and location.</p>\r\n                                <p>In the meantime, feel free to apply to other suitable positions on our careers page. Thank you for considering EPAM to be a part of your career journey.</p>\r\n                                <p>We wish you good luck in your job search and all future endeavors.</p>\r\n                                <p>Regards,</p>\r\n                                <p>Team EPAM India</p>\r\n                            </body>\r\n                            </html>", "Your application results" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailMessages");
        }
    }
}
