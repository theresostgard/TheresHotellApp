using HotellApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Data
{
    public class DataInitializer : IDataInitializer
    {
        public void MigrateAndSeed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();
            SeedGuests(dbContext);
            dbContext.SaveChanges();
            SeedRooms(dbContext);
            dbContext.SaveChanges();
        }

        private void SeedGuests(ApplicationDbContext dbContext)
        {
            if (!dbContext.Guest.Any()) 
            {
                dbContext.Guest.AddRange
                (
                new Guest { FirstName = "Jon", LastName = "Snow", PhoneNumber = "0732652651", EmailAdress = "jon.snow@GoT.com" },
                new Guest { FirstName = "Tyrion", LastName = "Lannister", PhoneNumber = "0735454748", EmailAdress = "tyrion.lannister@GoT.com" },
                new Guest { FirstName = "Daenerys", LastName = "Targaryen", PhoneNumber = "0707455478", EmailAdress = "daenerys.targaryen@GoT.com" },
                new Guest { FirstName = "Shireen", LastName = "Baratheon", PhoneNumber = "0768597584", EmailAdress = "shireen.baratheon@GoT.com" },
                new Guest { FirstName = "Samwell", LastName = "Tarly", PhoneNumber = "0705963587", EmailAdress = "sam.tarly@GoT.com" },
                new Guest { FirstName = "Jorah", LastName = "Mormont", PhoneNumber = "0768574236", EmailAdress = "jorah.mormont@GoT.com" },
                new Guest { FirstName = "Talisa", LastName = "Stark", PhoneNumber = "0761235426", EmailAdress = "talisa.stark@GoT.com" },
                new Guest { FirstName = "Grey", LastName = "Worm", PhoneNumber = "0709876985", EmailAdress = "grey.worm@GoT.com" }
                );
            };
                   
        }

        private void SeedRooms(ApplicationDbContext dbContext)
        {
            if(!dbContext.Room.Any())
            {
                dbContext.Room.AddRange
                (
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 25,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 2000m
                },
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Single,
                    RoomSize = 14,
                    IsExtraBedAllowed = false,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.None,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 1200m
                },
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 32,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.Two,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 2000m
                },
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 17,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 1800m
                },
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 25,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One,
                    Status = Models.Enums.StatusOfRoom.Active,
                    PricePerNight = 2100m
                },
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 14,
                    IsExtraBedAllowed = false,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.None,
                    Status = Models.Enums.StatusOfRoom.UnderMaintenance,
                    PricePerNight = 1400m
                },
                new Room
                {
                    RoomType = Models.Enums.TypeOfRoom.Double,
                    RoomSize = 42,
                    IsExtraBedAllowed = true,
                    AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.Two,
                    Status = Models.Enums.StatusOfRoom.InActive,
                    PricePerNight = 3500m
                },
                new Room
                {
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
}
