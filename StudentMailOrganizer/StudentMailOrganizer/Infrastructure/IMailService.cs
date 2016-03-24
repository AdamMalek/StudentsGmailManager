using StudentMailOrganizer.Models;
using System.Collections.Generic;

namespace StudentMailOrganizer.Infrastructure
{
    public interface IMailService
    {
        IEnumerable<MailMessage> GetAllMessages();
        bool SendMessage(MailMessage email);
        bool Login(string login, string password);
        bool IsLoggedIn();
        string GetCurrentLogin();
        bool Logout();
    }
}
