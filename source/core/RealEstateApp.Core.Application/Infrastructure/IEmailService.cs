namespace RealEstateApp.Core.Application.Interfaces.Infrastructure;

public interface IEmailService
{
    Task SendActivationEmailAsync(string toEmail, string firstName, string activationLink);
}