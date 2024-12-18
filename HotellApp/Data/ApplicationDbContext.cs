using HotellApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingRoom> BookingRoom { get; set; }

        public DbSet<Guest> Guest { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<Room> Room { get; set; }

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=TheresHotell;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konvertera TypeOfRoom till sträng
            modelBuilder.Entity<Room>()
                .Property(r => r.RoomType)
                .HasConversion<string>();

            // Konvertera StatusOfRoom till sträng
           modelBuilder.Entity<Room>()
                .Property(r => r.Status)
                .HasConversion<string>();

                // Konvertera AmountOfExtraBedsAllowedInRoom till sträng
            modelBuilder.Entity<Room>()
                .Property(r => r.AmountOfExtraBeds)
                .HasConversion<string>();
        }

    }
}
