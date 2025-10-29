using Ecommerce.Application.Models.Email;

namespace Ecommerce.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage emailMessage, string token);
}