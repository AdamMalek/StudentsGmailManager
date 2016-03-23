using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.Models
{
    public class MailMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public DateTime MailDate { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
