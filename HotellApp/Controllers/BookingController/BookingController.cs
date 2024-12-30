using HotellApp.Models.Enums;
using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotellApp.Services.BookingServices;
using HotellApp.Controllers.GuestController;
using HotellApp.Services.RoomServices;
using HotellApp.Utilities.ListDisplay;
using HotellApp.Utilities.BookingDisplay;
using HotellApp.Controllers.BookingCreationController;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace HotellApp.Controllers.BookingController
{
    public class BookingController : IBookingController
    {
        private readonly IBookingService _bookingService;
        private readonly IGuestController _guestController;
        private readonly IDisplayLists _displayLists;
        private readonly IDisplayBooking _displayBooking;
        private readonly IBookingCreationController _bookingCreationController;

        public BookingController(
            IBookingService bookingService,
            IGuestController guestController,
            IDisplayLists displayLists,
            IDisplayBooking displayBooking,
            IBookingCreationController bookingCreationController)
        {
            _bookingService = bookingService;
            _guestController = guestController;
            _displayLists = displayLists;
            _displayBooking = displayBooking;
            _bookingCreationController = bookingCreationController;
        }

        public void CreateBookingController()
        {
            bool continueBooking = true;

            while (continueBooking)
            {
                Console.Clear();
                int? guestId = null;
                while (guestId == null)
                {
                    var (guestType, selectedGuestId) = _guestController.SelectGuestType();

                    if (selectedGuestId == null)
                    {
                        Console.WriteLine("Ogiltigt kundnummer. Försök igen.\n");
                    }
                    else
                    {
                        guestId = selectedGuestId;
                    }
                }

                Console.WriteLine("Ange bokningsdetaljer:");

                var arrivalDate = _bookingCreationController.GetValidArrivalDate();
                var departureDate = _bookingCreationController.GetValidDepartureDate(arrivalDate);
                var (roomType, amountOfGuests, amountOfRooms, amountOfExtraBeds) = _bookingCreationController.GetRoomDetails();

                var availableRooms = _bookingCreationController.CheckRoomAvailability(roomType, arrivalDate, departureDate, amountOfGuests);

                if (availableRooms == null) return; 

                availableRooms = _bookingCreationController.FilterRoomsForExtraBeds(availableRooms, roomType == TypeOfRoom.Double);

                if (availableRooms.Count < amountOfRooms)
                {
                    Console.WriteLine("Det finns inte tillräckligt med rum som kan rymma extrasängar.");
                    return;
                }

                AnsiConsole.MarkupLine("[green]Tillgängliga rum att välja mellan[/]\n");
                _bookingCreationController.ShowAvailableRooms(availableRooms);

                List<Room> selectedRooms = new List<Room>();
                bool isValidSelection = false;

                while (!isValidSelection)
                {
                    selectedRooms = _bookingCreationController.SelectRooms(availableRooms, amountOfRooms);


                    if (selectedRooms.Count == amountOfRooms)
                    {
                        isValidSelection = true;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"\n[yellow]Du måste välja exakt {amountOfRooms} rum. Försök igen.[/]");
                    }
                }

                var booking = _bookingCreationController.CreateBooking(
                    guestId.Value,
                    arrivalDate,
                    departureDate,
                    roomType,
                    amountOfGuests,
                    selectedRooms.Count);

                _bookingCreationController.AssignRoomsToBooking(
                    selectedRooms,
                    booking,
                    arrivalDate,
                    departureDate);

                AnsiConsole.WriteLine($"Ny bokning skapad med bokningsnr {booking.BookingId}.");
                continueBooking = AnsiConsole.Confirm("Vill du skapa en till bokning?");
            }
        }

        public void ReadAllBookingsController()
        {

            var bookings = _bookingService.GetAllBookings(); 

            if (bookings == null || !bookings.Any())
            {
                AnsiConsole.MarkupLine("[red]Bokningen hittades inte![/]");
                return;
            }

            _displayBooking.Pagination(bookings); 
        }



        public void ReadBookingController()
        {


            bool IsContinuingReading = true;

            while (IsContinuingReading)
            {
                Console.Clear();
                _displayLists.DisplayBookings();

                var bookingId = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange bokningens ID: "));

                Console.Clear();
                var booking = _bookingService.ReadBooking(bookingId);

                if (booking != null)
                {
                    _displayBooking.DisplayBookingInformation(booking);
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Ingen bokning med bokningsnr[/] [green]{bookingId}[/] [red]hittades.[/]\n");
                }

                var continueOption = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Vill du se information om en annan bokning?")
                        .AddChoices("Ja", "Nej"));

                if (continueOption == "Nej")
                {
                    IsContinuingReading = false; 
                }
            }
        }

        public void UpdateBookingController()
        {
            bool isUpdatingBooking = true;
            while (isUpdatingBooking)
            {
                _displayLists.DisplayBookings();

                var bookingId = AnsiConsole.Ask<int>("Ange bokningsnummer för att uppdatera bokning:");
                var currentBooking = _bookingService.ReadBooking(bookingId);

                if (currentBooking == null)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("[red]Bokningen kunde inte hittas.[/]");
                }
                else
                {
                    if (currentBooking.ArrivalDate < DateTime.Now.Date)
                    {
                        Console.Clear();
                        AnsiConsole.MarkupLine("[red]Bokningen kan inte uppdateras eftersom den redan har inträffat.[/]");
                    }
                    else
                    {
                        Console.Clear();
                        var updatedBooking = GetBookingDetailsFromUser(currentBooking);

                        _bookingService.UpdateBooking(bookingId, updatedBooking);
                        AnsiConsole.MarkupLine("[green]Bokningen har uppdaterats![/]");
                    }
                }

                var continueUpdating = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Vill du uppdatera en till bokning?")
                        .AddChoices("Ja", "Nej"));

                isUpdatingBooking = continueUpdating == "Ja";
            }
        }

        public Booking GetBookingDetailsFromUser(Booking currentBooking)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]Uppdatera bokningsdetaljer[/]");

            _displayBooking.ShowCurrentBooking(currentBooking);

            AnsiConsole.MarkupLine("[gray]För att behålla det aktuella värdet, tryck bara på Enter.[/]");

            var arrivalDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>($"Ange nytt ankomstdatum (yyyy-MM-dd):")
                    .ValidationErrorMessage("[red]Ogiltigt datum. Datumet måste vara från idag eller senare.[/]")
                    .Validate(date => date >= DateTime.Today ? ValidationResult.Success() : ValidationResult.Error(
                        "[red]Ankomstdatum måste vara idag eller senare![/]")));

            var departureDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>($"Ange nytt avresedatum (yyyy-MM-dd):")
                    .ValidationErrorMessage("[red]Ogiltigt datum. Avresedatum måste vara efter ankomstdatum.[/]")
                    .Validate(date => date > arrivalDate ? ValidationResult.Success() : ValidationResult.Error(
                        "[red]Avresedatum måste vara efter ankomstdatum![/]")));


            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title($"Välj ny rumstyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            var amountOfGuests = AnsiConsole.Prompt(
                new TextPrompt<sbyte>($"Ange nytt antal gäster:")
                    .ValidationErrorMessage("[red]Antalet gäster måste vara minst 1![/]")
                    .Validate(guests => guests > 0 ? ValidationResult.Success() : ValidationResult.Error(
                        "[red]Antalet gäster måste vara större än 0![/]")));

            var amountOfRooms = AnsiConsole.Prompt(
                new TextPrompt<sbyte>($"Ange nytt antal rum:")
                    .ValidationErrorMessage("[red]Antalet rum måste vara minst 1![/]")
                    .Validate(rooms => rooms > 0 ? ValidationResult.Success() : ValidationResult.Error(
                        "[red]Antalet rum måste vara större än 0![/]")));

            _displayBooking.ShowUpdatedBookingSummary(
                currentBooking,
                arrivalDate,
                departureDate,
                roomType,
                amountOfGuests,
                amountOfRooms);

            Console.ReadKey();

            return new Booking
            {
                BookingId = currentBooking.BookingId,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                RoomType = roomType,
                AmountOfGuests = amountOfGuests,
                AmountOfRooms = amountOfRooms
            };
        }

        public void DeleteBookingController()
        {
            _displayLists.DisplayBookings();
            var id = AnsiConsole.Ask<int>("Ange bokningsnummer för den bokning du vill radera:");


            var confirm = AnsiConsole.Confirm("Är du säker på att du vill radera bokningen?");
            if (!confirm)
            {
                Console.WriteLine("Radering avbruten.");
                return;
            }

            var result = _bookingService.DeleteBooking(id);

            AnsiConsole.MarkupLine($"[yellow]{result}[/]");
            Console.ReadKey();
        }

    }
}
