using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.ViewModels
{
    public class MainWindowViewModel
    {
        public List<Category> Categories { get; set; }
        public List<MailMessage> CategoryItems { get; set; }
        public MailMessage SelectedMail { get; set; }
        public DateTime SelectedDate { get; set; }

        IMailService _mailer;
    }
}
