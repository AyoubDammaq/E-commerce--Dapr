using Microsoft.EntityFrameworkCore;
using CommandService.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CommandService.Data
{
    public class CommandDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public CommandDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CommandConnection"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);
        }


        public DbSet<Order> Orders { get; set; }
    }
}
