using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.ViewModels
{
    public class SynchMailViewModel
    {
        public List<MailMessage> Emails { get; set; }
        public List<Category> Categories { get; set; }
    }
}
