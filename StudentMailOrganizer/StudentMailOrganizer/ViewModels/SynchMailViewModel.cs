using StudentMailOrganizer.Models;
using System.Collections.Generic;

namespace StudentMailOrganizer.ViewModels
{
    public class SynchMailViewModel
    {
        public List<MailMessage> Emails { get; set; }
        public List<Category> Categories { get; set; }
    }
}
