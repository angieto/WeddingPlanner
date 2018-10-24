using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models 
{
    public class WeddingContext: DbContext
    {
        // base(options) calls the parent class's constructor
        public WeddingContext(DbContextOptions<WeddingContext> options): base(options) {}
        public DbSet<User> Users {get; set;} 
        public DbSet<Wedding> Weddings {get; set;}
        public DbSet<RSVP> RSVPs {get; set;}
    }
}