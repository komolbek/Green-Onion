using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DomainModels;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class UserDbContext : DbContext
    {

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }
}
