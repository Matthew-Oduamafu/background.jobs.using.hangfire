using BackgroundJobs.UsingHangfire.Api.Contracts;
using BackgroundJobs.UsingHangfire.Api.Models;
using BackgroundJobs.UsingHangfire.Api.Services;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.EntityFrameworkCore;
using HangfireDbContext = BackgroundJobs.UsingHangfire.Api.Data.HangfireDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HangfireDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
builder.Services.AddTransient<IEmailSender, EmailSender>();


// add Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    // use this for SQL Server 
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("Default"));
    // use this for SQLite storage 
    // config.UseSQLiteStorage(builder.Configuration.GetConnectionString("Default")); //.UseSQLiteStorage();
});
builder.Services.AddHangfireServer();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseHangfireDashboard();
app.MapHangfireDashboard();

// Recurring background Task
#pragma warning disable CS0618
RecurringJob.AddOrUpdate<IEmailSender>(x=>x.SendBatchMailSmtp(),"0 * * ? * *" );
#pragma warning restore CS0618

app.Run();