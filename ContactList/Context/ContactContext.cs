using ContactList.Models;
using Microsoft.EntityFrameworkCore;
namespace ContactList.Context
{
    public class ContactContext : DbContext
    {

        public ContactContext(DbContextOptions<ContactContext> options)
           : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Cat> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Cat>().HasData(
                new Cat { Id = 1, Name = "Friend" },
                new Cat { Id = 2, Name = "Work" },
                new Cat { Id = 3, Name = "Family" }
            );
        }
    }
}
