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


        public DbSet<Room> Room { get; set; }

        public DbSet<RoomStatusHistory> RoomStatusHistory { get; set; }

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

            modelBuilder.Entity<Guest>().HasData(
                new Guest { GuestId = 1, FirstName = "Jon", LastName = "Snow", PhoneNumber = "0732652651", EmailAdress = "jon.snow@GoT.com" },
                new Guest { GuestId = 2, FirstName = "Tyrion", LastName = "Lannister", PhoneNumber = "0735454748", EmailAdress = "tyrion.lannister@GoT.com" },
                new Guest { GuestId = 3, FirstName = "Daenerys", LastName = "Targaryen", PhoneNumber = "0707455478", EmailAdress = "daenerys.targaryen@GoT.com" },
                new Guest { GuestId = 4, FirstName = "Shireen", LastName = "Baratheon", PhoneNumber = "0768597584", EmailAdress = "shireen.baratheon@GoT.com" },
                new Guest { GuestId = 5, FirstName = "Samwell", LastName = "Tarly", PhoneNumber = "0705963587", EmailAdress = "sam.tarly@GoT.com" },
                new Guest { GuestId = 6, FirstName = "Jorah", LastName = "Mormont", PhoneNumber = "0768574236", EmailAdress = "jorah.mormont@GoT.com" },
                new Guest { GuestId = 7, FirstName = "Talisa", LastName = "Stark", PhoneNumber = "0761235426", EmailAdress = "talisa.stark@GoT.com" },
                new Guest { GuestId = 8, FirstName = "Grey", LastName = "Worm", PhoneNumber = "0709876985", EmailAdress = "grey.worm@GoT.com" }
                );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = 101,
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 25,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 2000m
                },
                new Room
                {
                    RoomId = 102,
                    RoomType = Models.Enums.TypeOfRoom.Single,
                    RoomSize = 14,
                    IsExtraBedAllowed = false,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.None,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 1200m
                },
                new Room
                {
                    RoomId = 103,
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 32,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.Two,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 2000m
                },
                new Room
                {
                    RoomId = 104,
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 17,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 1800m
                },
                new Room
                {
                    RoomId = 105,
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 25,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 2100m
                },
                new Room
                {
                    RoomId = 106,
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 14,
                    IsExtraBedAllowed = false,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.None,
                    Status = Models.Enums.StatusOfRoom.UnderMaintenance,
                    PricePerNight = 1400m
                },
                new Room
                {
                    RoomId = 107,
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 42,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.Two,
                    Status = Models.Enums.StatusOfRoom.InActive,
                    PricePerNight = 3500m
                },
                new Room
                {
                    RoomId = 108,
                    RoomType = Models.Enums.TypeOfRoom.Single,
                    RoomSize = 15,
                    IsExtraBedAllowed = false,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.None,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 1300m
                }
                );

        }

  

    }
}
