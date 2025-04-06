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
        public async Task<IActionResult> Index(int? pageIndex)
        {
            int pageSize = 5; // Кількість кімнат на сторінці
            var rooms = _db.HotelRooms.AsNoTracking(); // Використовуйте ваш DbSet
            var paginatedRooms = await PaginatedList<HotelRoom>.CreateAsync(rooms, pageIndex ?? 1, pageSize);
            return View(paginatedRooms);
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
