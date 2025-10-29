using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using FluentEmail.Core;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.MessageImplementation;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailFluentSettings _emailFluentSettings;

    public EmailService(IFluentEmail fluentEmail, IOptions<EmailFluentSettings> emailFluentSettings)
    {
        _fluentEmail = fluentEmail;
        _emailFluentSettings = emailFluentSettings.Value;
    }

    public async Task<bool> SendEmailAsync(EmailMessage emailMessage, string token)
    {
        var htmlContent = $"{emailMessage.Body} {_emailFluentSettings.BaseUrlClient}/password/reset/{token}";
        var result = await _fluentEmail
            .To(emailMessage.To)
            .Subject(emailMessage.Subject)
            .Body(htmlContent)
            .SendAsync();

        return result.Successful;
    }
}