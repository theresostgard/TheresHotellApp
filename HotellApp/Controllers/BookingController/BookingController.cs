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

namespace HotellApp.Controllers.BookingController
{
    public class BookingController : IBookingController
    {
        private readonly IBookingService _bookingService;
        private readonly IGuestController _guestController;
        private readonly IRoomService _roomService;
        private readonly IDisplayLists _displayLists;

        public BookingController(IBookingService bookingService,
            IGuestController guestController,
            IRoomService roomService,
            IDisplayLists displayLists)
        {
            _bookingService = bookingService;
            _guestController = guestController;
            _roomService = roomService;
            _displayLists = displayLists;
        }

        public void CreateBookingController()
        {
            Console.Clear();
            var (guestType, guestId) = _guestController.SelectCustomerType();

            if (guestId == null)
            {
                Console.WriteLine("Kundval kunde inte genomföras. Avslutar bokningen.");
                return;
            }

            Console.WriteLine("Ange bokningsdetaljer:");

            var arrivalDate = GetValidArrivalDate();
            var departureDate = GetValidDepartureDate(arrivalDate);
            var (roomType, amountOfGuests, amountOfRooms, amountOfExtraBeds) = GetRoomDetails();

            // Steg 3: Kontrollera tillgängliga rum
            var availableRooms = CheckRoomAvailability(roomType, arrivalDate, departureDate, amountOfGuests);

            if (availableRooms == null) return;  // Om det inte finns tillräckligt med tillgängliga rum

            // Steg 4: Filtrera rummen (om det behövs extrasäng)
            availableRooms = FilterRoomsForExtraBeds(availableRooms, roomType == TypeOfRoom.Double);

            if (availableRooms.Count < amountOfRooms)
            {
                Console.WriteLine("Det finns inte tillräckligt med rum som kan rymma extrasängar.");
                return;
            }

            // Steg 5: Visa tillgängliga rum
            AnsiConsole.MarkupLine("[green]Tillgängliga rum att välja mellan[/]\n");
            ShowAvailableRooms(availableRooms);

            // Steg 6: Låt användaren välja rum
            var selectedRooms = SelectRooms(availableRooms, amountOfRooms);

            if (selectedRooms.Count < amountOfRooms)
            {
                AnsiConsole.MarkupLine($"\n[yellow]Välj minst {amountOfRooms} rum.[/]");
                return;
            }

            // Steg 7: Skapa bokning
            var booking = CreateBooking(guestId.Value, 
                arrivalDate, 
                departureDate, 
                roomType, 
                amountOfGuests, 
                selectedRooms.Count);

            // Steg 8: Koppla rummen till bokningen och uppdatera rumsstatus
            AssignRoomsToBooking(selectedRooms, 
                                booking, 
                                arrivalDate, 
                                departureDate);

            AnsiConsole.WriteLine($"Ny bokning skapad med bokningsnr {booking.BookingId}.");

        }


        private DateTime GetValidArrivalDate()
        {
            DateTime arrivalDate;
            do
            {
                arrivalDate = AnsiConsole.Ask<DateTime>("Ankomstdatum (yyyy-MM-dd):");
                if (arrivalDate < DateTime.Now.Date)
                {
                    Console.WriteLine("Ankomstdatumet måste vara ett framtida datum. Försök igen.");
                }
            } while (arrivalDate < DateTime.Now.Date);
            return arrivalDate;
        }

        private DateTime GetValidDepartureDate(DateTime arrivalDate)
        {
            DateTime departureDate;
            do
            {
                departureDate = AnsiConsole.Ask<DateTime>("Avresedatum (yyyy-MM-dd):");
                if (departureDate <= arrivalDate)
                {
                    Console.WriteLine("Avresedatumet måste vara efter ankomstdatumet. Försök igen.");
                }
            } while (departureDate <= arrivalDate);
            return departureDate;
        }

        private (TypeOfRoom roomType, sbyte amountOfGuests, sbyte amountOfRooms, int amountOfExtraBeds) GetRoomDetails()
        {
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title("Välj rumsyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            var amountOfGuests = AnsiConsole.Ask<sbyte>("Antal gäster: ");
            var amountOfRooms = AnsiConsole.Ask<sbyte>("Antal rum: ");

            int amountOfExtraBeds = 0;

            // Fråga om extrasängar om rummet är av typen Double
            if (roomType == TypeOfRoom.Double)
            {
                var isExtraBedWanted = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Önskas extrasäng?")
                        .AddChoices("Ja", "Nej")
                ) == "Ja";

                if (isExtraBedWanted)
                {
                    amountOfExtraBeds = AnsiConsole.Ask<int>("Hur många extrasängar önskas?");
                }
            }
            return (roomType, amountOfGuests, amountOfRooms, amountOfExtraBeds);
        }
        private List<Room> CheckRoomAvailability(TypeOfRoom roomType, 
            DateTime arrivalDate, 
            DateTime departureDate, 
            sbyte amountOfRooms)
        {
            var availableRooms = _roomService.GetAvailableRooms(
                roomType, 
                arrivalDate, 
                departureDate, 
                amountOfRooms);

            if (availableRooms == null || availableRooms.Count < amountOfRooms)
            {
                Console.WriteLine("Det finns inte tillräckligt med tillgängliga rum för den valda rumstypen och datumintervallet.");
                return null;
            }
            return availableRooms;
        }

        private List<Room> FilterRoomsForExtraBeds(List<Room> availableRooms, bool isExtraBedAllowed)
        {
            if (isExtraBedAllowed)
            {
                availableRooms = availableRooms
                    .Where(r => r.RoomSize >= 15)
                    .ToList();
            }
            return availableRooms;
        }

        private void ShowAvailableRooms(List<Room> availableRooms)
        {
            var table = new Table();
            table.AddColumn("Rum #");
            table.AddColumn("Typ");
            table.AddColumn("Status");
            table.AddColumn("Storlek");

            foreach (var room in availableRooms)
            {
                table.AddRow(room.RoomId.ToString(), 
                    room.RoomType.ToString(), 
                    room.Status.ToString(), 
                    room.RoomSize.ToString());
            }

            AnsiConsole.Write(table);
        }
        private List<Room> SelectRooms(List<Room> availableRooms, sbyte amountOfRooms)
        {
            return AnsiConsole.Prompt(
                new MultiSelectionPrompt<Room>()
                    .Title("Välj de rum du vill boka:")
                    .AddChoices(availableRooms)
                    .UseConverter(room => $"{room.RoomId} - " +
                    $"{room.RoomType} " +
                    $"({room.RoomSize}m², " +
                    $"{room.Status})"))
                    .Take(amountOfRooms)
                    .ToList();
        }
        private Booking CreateBooking(int guestId, 
            DateTime arrivalDate, 
            DateTime departureDate, 
            TypeOfRoom roomType, 
            sbyte amountOfGuests, 
            int amountOfRooms)
        {
            var booking = new Booking
            {
                GuestId = guestId,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                RoomType = roomType,
                AmountOfGuests = amountOfGuests,
                AmountOfRooms = (sbyte)amountOfRooms
            };

            _bookingService.CreateBooking(booking);
            return booking;
        }
        private void AssignRoomsToBooking(List<Room> selectedRooms, 
            Booking booking, 
            DateTime arrivalDate, 
            DateTime departureDate)
        {
            var bookingRooms = selectedRooms.Select(room => new BookingRoom
            {
                RoomId = room.RoomId,
                BookingId = booking.BookingId
            }).ToList();

            _bookingService.AddRoomsToBooking(bookingRooms);

            foreach (var room in selectedRooms)
            {
                _roomService.ChangeRoomStatusForDateRange(room.RoomId, 
                    StatusOfRoom.Reserved, 
                    arrivalDate, 
                    departureDate);
            }
        }

        public void ReadAllBookingsController()
        {
            var bookings = _bookingService.GetAllBookings();  // Hämtar alla bokningar

            if (bookings != null && bookings.Any())  // Kontrollera om det finns några bokningar
            {
                foreach (var booking in bookings)
                {
                    // Kontrollera om Booking.Guest inte är null
                    var guestName = booking.Guest != null ? $"{booking.Guest.FirstName} {booking.Guest.LastName}" : "Ingen gäst kopplad";

                    DisplayBooking.DisplayBookingInformation(booking);

                    bool noRoomConnectedToBooking = booking.BookingRooms == null || !booking.BookingRooms.Any();

                    if (noRoomConnectedToBooking)
                    {
                        Console.WriteLine("Inga rum kopplade till denna bokning.");
                        continue;
                    }
                    foreach (var bookingRoom in booking.BookingRooms)
                    {
                        if (bookingRoom.Room != null)
                        {

                            var room = bookingRoom.Room;
                            var roomInfo = new Markup($"[yellow]RumsNr: {room.RoomId}[/]\n" +
                                                       $"[yellow]Rumstyp: {room.RoomType}[/]\n" +
                                                       $"[yellow]Storlek: {room.RoomSize} kvm[/]\n");

                            AnsiConsole.Write(
                            new Panel(roomInfo)
                            .BorderColor(Color.Red) 
                            .Header($"Bokningsnr {booking.BookingId}") 
                            .Border(BoxBorder.Square)
                            );
                        }
                        else if (!noRoomConnectedToBooking)
                        {
                            Console.WriteLine("Rum kopplat till bokningen saknas.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Inga bokningar hittades.");
            }

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
                    DisplayBooking.DisplayBookingInformation(booking);
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
                    IsContinuingReading = false;  // Stäng av loopen om användaren inte vill fortsätta
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
                    AnsiConsole.MarkupLine("[red]Bokningen kunde inte hittas.[/]");
                }
                else
                {
                    var updatedBooking = GetBookingDetailsFromUser(currentBooking);

                    _bookingService.UpdateBooking(bookingId, updatedBooking);
                    AnsiConsole.MarkupLine("[green]Bokningen har uppdaterats![/]");
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
            // Hämta och validera ankomstdatum
            var arrivalDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>($"Nuvarande ankomstdatum: {currentBooking.ArrivalDate:yyyy-MM-dd}.\nAnge nytt ankomstdatum (yyyy-MM-dd):")
                    .ValidationErrorMessage("[red]Ogiltigt datum. Datumet måste vara från idag eller senare.[/]")
                    .Validate(date => date >= DateTime.Today ? ValidationResult.Success() : ValidationResult.Error("[red]Ankomstdatum måste vara idag eller senare![/]")));

            // Hämta och validera avresedatum
            var departureDate = AnsiConsole.Prompt(
                new TextPrompt<DateTime>($"Nuvarande avresedatum: {currentBooking.DepartureDate:yyyy-MM-dd}.\nAnge nytt avresedatum (yyyy-MM-dd):")
                    .ValidationErrorMessage("[red]Ogiltigt datum. Avresedatum måste vara efter ankomstdatum.[/]")
                    .Validate(date => date > arrivalDate ? ValidationResult.Success() : ValidationResult.Error("[red]Avresedatum måste vara efter ankomstdatum![/]")));

            // Hämta och uppdatera rumstyp
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title($"Nuvarande rumstyp: {currentBooking.RoomType}. Välj ny rumstyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            // Hämta och validera antal gäster
            var amountOfGuests = AnsiConsole.Prompt(
                new TextPrompt<sbyte>($"Nuvarande antal gäster: {currentBooking.AmountOfGuests}. Ange nytt antal gäster:")
                    .ValidationErrorMessage("[red]Antalet gäster måste vara minst 1![/]")
                    .Validate(guests => guests > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Antalet gäster måste vara större än 0![/]")));

            // Hämta och validera antal rum
            var amountOfRooms = AnsiConsole.Prompt(
                new TextPrompt<sbyte>($"Nuvarande antal rum: {currentBooking.AmountOfRooms}. Ange nytt antal rum:")
                    .ValidationErrorMessage("[red]Antalet rum måste vara minst 1![/]")
                    .Validate(rooms => rooms > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Antalet rum måste vara större än 0![/]")));

            // Returnera uppdaterad bokning
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
            //visa lista med bokningar och datum så man vet vad man kan välja 
            var id = AnsiConsole.Ask<int>("Ange bokningsnummer för den bokning du vill radera:");

            var confirm = AnsiConsole.Confirm("Är du säker på att du vill radera bokningen?");
            if (!confirm)
            {
                Console.WriteLine("Radering avbruten.");
                return;
            }

            // Kalla på servicen för att försöka ta bort bokningen
            var result = _bookingService.DeleteBooking(id);

            // Visa resultatet till användaren
            AnsiConsole.MarkupLine($"[yellow]{result}[/]");
        }

      
    }
}
