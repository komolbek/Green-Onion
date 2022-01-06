using Microsoft.EntityFrameworkCore;
using GreenOnion.DomainModels;

namespace GreenOnion.Server.Datalayer.Dataaccess
{
    public class UserDbContext : DbContext
    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }
}
