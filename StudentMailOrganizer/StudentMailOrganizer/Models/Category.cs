using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public IEnumerable<MailMessage> CategoryItems { get; set; }
        public IEnumerable<string> AcceptedEmails { get; set; }
    }
}
