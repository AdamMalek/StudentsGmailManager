using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace StudentMailOrganizer.Infrastructure
{
    public interface IMailService
    {
        IEnumerable<MailMessage> GetAllMessages();
        bool SendMessage(MailMessage email);
        bool Login(string login, SecureString password);
        bool IsLoggedIn();
    }
}
