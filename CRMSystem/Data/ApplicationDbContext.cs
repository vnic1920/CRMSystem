using CRMSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Kontakt> Kontakte { get; set; }
        public DbSet<Aufgabe> Aufgaben { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Kontakt>()
                .HasOne(k => k.Kunde)
                .WithMany(k => k.Kontakte)
                .HasForeignKey(k => k.KundeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Aufgabe>()
                .HasOne(a => a.Kunde)
                .WithMany()
                .HasForeignKey(a => a.KundeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}