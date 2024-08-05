namespace CORE.Abstract;

public interface IMailService
{
    public Task SendMailAsync(string email, string message);
}
