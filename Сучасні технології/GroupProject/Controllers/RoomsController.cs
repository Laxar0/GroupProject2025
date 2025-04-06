using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;
using GroupProject.ViewModels;

namespace GroupProject.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoomsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Вивід списку номерів з демонстраційними даними
        public IActionResult Index()
        {
            if (!_db.HotelRooms.Any())
            {
                _db.HotelRooms.AddRange(new List<HotelRoom>
                {
                    new HotelRoom
                    {
                        Name = "Номер Люкс",
                        Description = "Великий номер з видом на місто",
                        PricePerNight = 1200,
                        Capacity = 2
                    },
                    new HotelRoom
                    {
                        Name = "Економ",
                        Description = "Бюджетний номер без вікна",
                        PricePerNight = 500,
                        Capacity = 1
                    }
                });
                _db.SaveChanges();
            }

            var rooms = _db.HotelRooms.ToList();
            return View(rooms);
        }

        // GET: Rooms/Book/1
        [HttpGet]
        public IActionResult Book(int id)
        {
            Console.WriteLine($"▶️ GET Rooms/Book/{id}");

            var room = _db.HotelRooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new RoomBookingViewModel
            {
                Room = room,
                HotelRoomId = room.Id
            };

            return View(viewModel);
        }

        // POST: Rooms/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(RoomBookingViewModel model)
        {
            Console.WriteLine("▶️ POST Rooms/Book");

            Console.WriteLine($"HotelRoomId: {model.HotelRoomId}");
            Console.WriteLine($"GuestName: {model.GuestName}");
            Console.WriteLine($"GuestEmail: {model.GuestEmail}");
            Console.WriteLine($"CheckIn: {model.CheckInDate}");
            Console.WriteLine($"CheckOut: {model.CheckOutDate}");

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("❌ Model error: " + error.ErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                model.Room = _db.HotelRooms.FirstOrDefault(r => r.Id == model.HotelRoomId);
                return View(model);
            }

            var booking = new Booking
            {
                HotelRoomId = model.HotelRoomId,
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate
            };

            _db.Bookings.Add(booking);
            var saved = _db.SaveChanges();

            Console.WriteLine($"✅ Збережено записів у базу: {saved}");

            return RedirectToAction(nameof(Index));
        }
    }
}
