namespace BusinessLogicLayer.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string url);
    }
}
