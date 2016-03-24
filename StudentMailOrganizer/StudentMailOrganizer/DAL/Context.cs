using StudentMailOrganizer.Models;
using System.Data.Entity;

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
