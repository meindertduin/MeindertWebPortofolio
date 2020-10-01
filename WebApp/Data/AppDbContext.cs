using System;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<BigProject> BigProjects { get; set; }
        public DbSet<BigProjectImage> BigProjectImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BigProjectImage>()
                .HasOne(x => x.BigProject)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.BigProjectId);

            modelBuilder.Entity<BigProject>()
                .Property(x => x.Features)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}