using System.Collections.Generic;

namespace StudentMailOrganizer.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public virtual ICollection<Sender> AcceptedEmails { get; set; }
        public virtual ICollection<MailMessage> Mails { get; set; }
    }
}
