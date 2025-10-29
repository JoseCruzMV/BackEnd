using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SendEmail()
    {
        var message = new EmailMessage()
        {
            To = "a@mail.com",
            Body = "Prueba de email con token",
            Subject = "Cambiar contraseña"
        };

        var result = await _emailService.SendEmailAsync(message, "Token");

        return result ? Ok() : BadRequest();
    }
}