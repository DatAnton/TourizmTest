using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TourizmTest.Models 
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Service> Services { get;set; }
        public DbSet<PhotoFile> PhotoFiles { get;set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     modelBuilder.Entity<Service>()
        //     .HasKey(p => new { p.Id });   
        //     modelBuilder.Entity<PhotoFile>()
        //     .HasKey(p => new { p.Id });         
        // }
    }
}