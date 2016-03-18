using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.DAL
{
    public class MailContext : DbContext
    {
        public DbSet<MailMessage> Emails { get; set; }
        public DbSet<DbCategory> Categories { get; set; }

        public MailContext()
        {
            Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MailContext>());
        }
    }
}
