using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database
{
    public class RapidusContext : DbContext
    {
        public RapidusContext(DbContextOptions options) : base(options) { }

        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleTypeSpecifications> VehicleTypeSpecifications { get; set; }
        public DbSet<SiteVehicleType> VehicleTypeSiteAssociation { get; set; }
        public DbSet<Bid> Bid { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<FleetOwner> FleetOwner { get; set; }
        public DbSet<Installer> Installer { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<JobImages> JobImages { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<SiteStatusLog> SiteStatusLog { get; set; }
        //public DbSet<State> State { get; set; }
        public DbSet<SupervisorSiteAssociation> SupervisorSiteAssociation { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CorporateUser> CorporateUser { get; set; }
        public DbSet<Supervisor> Supervisor { get; set; }
        public DbSet<Crew> Crew { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<CrewSite> CrewSites { get; set; }
        public DbSet<JobStatusLogs> JobStatusLogs { get; set; }
        public DbSet<RejectedJob> RejectedJobs { get; set; }

        public DbSet<EmailConfigurations> EmailConfigurations { get; set; }

    }

    public class RapidusContextFactory : IDesignTimeDbContextFactory<RapidusContext>
    {
        public RapidusContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<RapidusContext>()
           // .UseMySql("server=192.168.5.60;port=3306;database=rapidus;user=rapidus;password=1234");// not used VU360Solutions
          //  .UseMySql("server=localhost;port=3306;database=rapidusTest;user=root;password=root");//shahid test
           .UseMySql("server=localhost;port=3306;database=rapidus;user=root;password=root");
           //  .UseMySql("server=34.73.221.148;port=3306;database=rapidus;user=rapidus;password=VU360Solutions");
           // .UseMySql("server=34.73.221.148;port=3306;database=rapidus_live;user=rapidus;password=VU360Solutions");
            return new RapidusContext(options.Options);
        }
    }
}
