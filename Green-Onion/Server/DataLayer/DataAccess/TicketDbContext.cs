using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class TicketDbContext : DbContext
        {

        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {

        }

        public DbSet<Ticket> tickets { get; set; }
    }
}
