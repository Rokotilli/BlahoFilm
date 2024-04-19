using BusinessLogicLayer.Models.Enums;

namespace BusinessLogicLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string content, SendEmailActions action);
    }
}
