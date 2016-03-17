using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.Models
{
    public class MailMessage
    {
        public int MessageId { get; set; }
        public string Sender { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
    }
}
