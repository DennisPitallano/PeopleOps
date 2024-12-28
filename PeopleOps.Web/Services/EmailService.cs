using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Liquid;
using FluentEmail.Smtp;
using Microsoft.Extensions.Options;

namespace PeopleOps.Web.Services;

public class EmailService : IEmailService
{
    private readonly EmailSetting _emailSetting;

    public EmailService(IOptions<EmailSetting> emailSetting)
    {
        _emailSetting = emailSetting.Value;
        Email.DefaultRenderer = new LiquidRenderer(Options.Create(new LiquidRendererOptions()));
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var sender = new SmtpSender(() => new SmtpClient(_emailSetting.Host)
        {
            Port = _emailSetting.Port,
            Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password),
            EnableSsl = _emailSetting.EnableSsl
        });

        Email.DefaultSender = sender;

        var email = await Email
            .From(_emailSetting.SenderEmail, _emailSetting.SenderName)
            .To(to)
            .Subject(subject)
            .UsingTemplateFromFile("Templates/PointsNotificationTemplate.liquid",
                new { name = "Adventurer", points = 100, cards = 1 })
            .SendAsync();
    }
}