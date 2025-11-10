using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRMSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Kontakt> Kontakte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kunde Configuration
            modelBuilder.Entity<Kunde>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Telefon).HasMaxLength(50);
                entity.Property(e => e.Firma).HasMaxLength(100);
                entity.Property(e => e.Adresse).HasMaxLength(200);
                entity.Property(e => e.Notizen).HasMaxLength(1000);
            });

            // Kontakt Configuration
            modelBuilder.Entity<Kontakt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Telefon).HasMaxLength(50);
                entity.Property(e => e.Position).HasMaxLength(100);
                entity.Property(e => e.Notizen).HasMaxLength(500);

                // Relationship with Kunde
                entity.HasOne(k => k.Kunde)
                      .WithMany(k => k.Kontakte)
                      .HasForeignKey(k => k.KundeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}