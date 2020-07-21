using CodeChallenge.API.Models;
using Microsoft.EntityFrameworkCore;


namespace CodeChallenge.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<server_reponse_log>(eb =>
                {
                    eb.HasNoKey();
                });
        }
        public DbSet<server_reponse_log> server_reponse_log { get; set; }
     }
}