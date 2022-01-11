using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DomainModels;

namespace GreenOnion.Server
{
    public class GreenOnionContext : DbContext
    {

        public GreenOnionContext(DbContextOptions<GreenOnionContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Ticket> tickets{ get; set; }
        public DbSet<Project> projects{ get; set; }
        public DbSet<Company> companies { get; set; }
    }
}
