using StudentMailOrganizer.Models;
using System.Configuration;
using System.Data.Entity;
using System.IO;

namespace StudentMailOrganizer.DAL
{
    public class MailContext : DbContext
    {
        public DbSet<MailMessage> Emails { get; set; }
        public DbSet<Category> Categories { get; set; }

        public MailContext()
        {
            Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MailContext>());
        }
    }
}
