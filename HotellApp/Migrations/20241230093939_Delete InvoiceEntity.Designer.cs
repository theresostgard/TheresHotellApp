﻿// <auto-generated />
using System;
using HotellApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotellApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241230093939_Delete InvoiceEntity")]
    partial class DeleteInvoiceEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HotellApp.Models.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<int>("AmountOfExtraBeds")
                        .HasColumnType("int");

                    b.Property<short>("AmountOfGuests")
                        .HasColumnType("smallint");

                    b.Property<short>("AmountOfRooms")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookingId");

                    b.HasIndex("GuestId");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("HotellApp.Models.BookingRoom", b =>
                {
                    b.Property<int>("BookingRoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingRoomId"));

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("BookingRoomId");

                    b.HasIndex("BookingId");

                    b.HasIndex("RoomId");

                    b.ToTable("BookingRoom");
                });

            modelBuilder.Entity("HotellApp.Models.Guest", b =>
                {
                    b.Property<int>("GuestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GuestId"));

                    b.Property<string>("EmailAdress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GuestStatus")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GuestId");

                    b.ToTable("Guest");

                    b.HasData(
                        new
                        {
                            GuestId = 1,
                            EmailAdress = "jon.snow@GoT.com",
                            FirstName = "Jon",
                            GuestStatus = 0,
                            LastName = "Snow",
                            PhoneNumber = "0732652651"
                        },
                        new
                        {
                            GuestId = 2,
                            EmailAdress = "tyrion.lannister@GoT.com",
                            FirstName = "Tyrion",
                            GuestStatus = 0,
                            LastName = "Lannister",
                            PhoneNumber = "0735454748"
                        },
                        new
                        {
                            GuestId = 3,
                            EmailAdress = "daenerys.targaryen@GoT.com",
                            FirstName = "Daenerys",
                            GuestStatus = 0,
                            LastName = "Targaryen",
                            PhoneNumber = "0707455478"
                        },
                        new
                        {
                            GuestId = 4,
                            EmailAdress = "shireen.baratheon@GoT.com",
                            FirstName = "Shireen",
                            GuestStatus = 0,
                            LastName = "Baratheon",
                            PhoneNumber = "0768597584"
                        },
                        new
                        {
                            GuestId = 5,
                            EmailAdress = "sam.tarly@GoT.com",
                            FirstName = "Samwell",
                            GuestStatus = 0,
                            LastName = "Tarly",
                            PhoneNumber = "0705963587"
                        },
                        new
                        {
                            GuestId = 6,
                            EmailAdress = "jorah.mormont@GoT.com",
                            FirstName = "Jorah",
                            GuestStatus = 0,
                            LastName = "Mormont",
                            PhoneNumber = "0768574236"
                        },
                        new
                        {
                            GuestId = 7,
                            EmailAdress = "talisa.stark@GoT.com",
                            FirstName = "Talisa",
                            GuestStatus = 0,
                            LastName = "Stark",
                            PhoneNumber = "0761235426"
                        },
                        new
                        {
                            GuestId = 8,
                            EmailAdress = "grey.worm@GoT.com",
                            FirstName = "Grey",
                            GuestStatus = 0,
                            LastName = "Worm",
                            PhoneNumber = "0709876985"
                        });
                });

            modelBuilder.Entity("HotellApp.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"));

                    b.Property<string>("AmountOfExtraBeds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsExtraBedAllowed")
                        .HasColumnType("bit");

                    b.Property<decimal>("PricePerNight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RoomSize")
                        .HasColumnType("int");

                    b.Property<string>("RoomType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomId");

                    b.ToTable("Room");

                    b.HasData(
                        new
                        {
                            RoomId = 101,
                            AmountOfExtraBeds = "One",
                            IsExtraBedAllowed = true,
                            PricePerNight = 2000m,
                            RoomSize = 25,
                            RoomType = "Double",
                            Status = "Active"
                        },
                        new
                        {
                            RoomId = 102,
                            AmountOfExtraBeds = "None",
                            IsExtraBedAllowed = false,
                            PricePerNight = 1200m,
                            RoomSize = 14,
                            RoomType = "Single",
                            Status = "Active"
                        },
                        new
                        {
                            RoomId = 103,
                            AmountOfExtraBeds = "Two",
                            IsExtraBedAllowed = true,
                            PricePerNight = 2000m,
                            RoomSize = 32,
                            RoomType = "Double",
                            Status = "Active"
                        },
                        new
                        {
                            RoomId = 104,
                            AmountOfExtraBeds = "One",
                            IsExtraBedAllowed = true,
                            PricePerNight = 1800m,
                            RoomSize = 17,
                            RoomType = "Double",
                            Status = "Active"
                        },
                        new
                        {
                            RoomId = 105,
                            AmountOfExtraBeds = "One",
                            IsExtraBedAllowed = true,
                            PricePerNight = 2100m,
                            RoomSize = 25,
                            RoomType = "Double",
                            Status = "Active"
                        },
                        new
                        {
                            RoomId = 106,
                            AmountOfExtraBeds = "None",
                            IsExtraBedAllowed = false,
                            PricePerNight = 1400m,
                            RoomSize = 14,
                            RoomType = "Double",
                            Status = "UnderMaintenance"
                        },
                        new
                        {
                            RoomId = 107,
                            AmountOfExtraBeds = "Two",
                            IsExtraBedAllowed = true,
                            PricePerNight = 3500m,
                            RoomSize = 42,
                            RoomType = "Double",
                            Status = "InActive"
                        },
                        new
                        {
                            RoomId = 108,
                            AmountOfExtraBeds = "None",
                            IsExtraBedAllowed = false,
                            PricePerNight = 1300m,
                            RoomSize = 15,
                            RoomType = "Single",
                            Status = "Active"
                        });
                });

            modelBuilder.Entity("HotellApp.Models.RoomStatusHistory", b =>
                {
                    b.Property<int>("RoomStatusHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomStatusHistoryId"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("RoomStatusHistoryId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomStatusHistory");
                });

            modelBuilder.Entity("HotellApp.Models.Booking", b =>
                {
                    b.HasOne("HotellApp.Models.Guest", "Guest")
                        .WithMany("Bookings")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("HotellApp.Models.BookingRoom", b =>
                {
                    b.HasOne("HotellApp.Models.Booking", "Booking")
                        .WithMany("BookingRooms")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HotellApp.Models.Room", "Room")
                        .WithMany("BookingRooms")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotellApp.Models.RoomStatusHistory", b =>
                {
                    b.HasOne("HotellApp.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("HotellApp.Models.Booking", b =>
                {
                    b.Navigation("BookingRooms");
                });

            modelBuilder.Entity("HotellApp.Models.Guest", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("HotellApp.Models.Room", b =>
                {
                    b.Navigation("BookingRooms");
                });
#pragma warning restore 612, 618
        }
    }
}
