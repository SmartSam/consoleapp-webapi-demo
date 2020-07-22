using CodeChallenge.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace CodeChallenge.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<server_response_log>(eb =>
                {
                    // https://github.com/dotnet/EntityFramework.Docs/issues/898
                    //if (Database.IsSqlServer())
                    //    eb.HasNoKey();
                    //else
                    eb.HasKey(e => e.Starttime);
                });
            modelBuilder.Entity<recentResponseLog>(eb =>
                 {
                 eb.HasNoKey();
             });

            modelBuilder.Entity<errorCodeLog>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("ServerResponseErrors");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 optionsBuilder.UseSqlServer("Server=.;Database=ae_code_challenge;Trusted_Connection=True;");
            }
        }
        public DbSet<server_response_log> server_response_log { get; set; }
        public DbSet<errorCodeLog> errorCodeLogs { get; set; }
        public DbSet<recentResponseLog> recentResponseLogs { get; set; }
    }
}