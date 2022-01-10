using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DomainModels;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class CompanyDbContext : DbContext
    {

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) 
        {
        }

        public DbSet<Company> companies { get; set; }
    }
}
