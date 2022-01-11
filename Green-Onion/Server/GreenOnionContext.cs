using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DomainModels;

namespace GreenOnion.Server
{
    public class GreenOnionContext : DbContext
    {

        public GreenOnionContext(DbContextOptions<GreenOnionContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<UserAccount> User_account { get; set; }

        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyEmployee> Company_employee { get; set; }

        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectMember> Project_member { get; set; }

        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketAssignee> Ticket_assignee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=..//..//Database/GreenOnion.db");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyEmployee>()
                .HasKey(compEmpl => new { compEmpl.companyId, compEmpl.userId });

            modelBuilder.Entity<ProjectMember>()
                .HasKey(projMem => new { projMem.projectId, projMem.userId });

            modelBuilder.Entity<TicketAssignee>()
                .HasKey(tickAss => new { tickAss.ticketId, tickAss.userId });
        }
    }
}
