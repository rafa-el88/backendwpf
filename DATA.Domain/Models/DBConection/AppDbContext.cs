using System;
using System.Linq;
using DATA.Domain.Models;
using Microsoft.EntityFrameworkCore;  

namespace DATA.Domain.Models
{
    public partial class AppDbContext : DbContext
    { 
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
         
        public DbSet<User> Users { get; set; }  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasKey(p => p.Id);
            }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).ModificadoEm = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CriadoEm = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}