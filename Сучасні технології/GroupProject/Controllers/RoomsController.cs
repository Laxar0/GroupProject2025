using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GroupProject.Models;
using GroupProject.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GroupProject.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

<<<<<<< Updated upstream
        public async Task<IActionResult> Index(int? pageIndex)
        {
            const int pageSize = 5; // Кількість кімнат на сторінці
=======
<<<<<<< HEAD
        public async Task<IActionResult> Index(int? pageIndex)
        {
            const int pageSize = 5; // Кількість кімнат на сторінці
=======
        public IActionResult Index()
        {
            if (!_db.HotelRooms.Any())
            {
                _db.HotelRooms.AddRange(new List<HotelRoom>
                {
                    new HotelRoom { Name = "Номер Люкс", Description = "Великий номер з видом на місто", PricePerNight = 1200, Capacity = 2 },
                    new HotelRoom { Name = "Економ", Description = "Бюджетний номер без вікна", PricePerNight = 500, Capacity = 1 }
                });
                _db.SaveChanges();
            }
>>>>>>> 1fe4b4eba183c332773b028d119fc51f39dced1e
>>>>>>> Stashed changes

            // Отримуємо всі номери з бази даних
            var roomsQuery = _db.HotelRooms
                .OrderBy(r => r.RoomNumber) // Сортуємо за номером кімнати
                .AsNoTracking();

            var paginatedRooms = await PaginatedList<HotelRoom>.CreateAsync(
                roomsQuery,
                pageIndex ?? 1,
                pageSize);

            return View(paginatedRooms);
        }

        [HttpGet]
        public IActionResult Book(int id)
        {
            var room = _db.HotelRooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();

            var viewModel = new RoomBookingViewModel
            {
                Room = room,
                HotelRoomId = room.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(RoomBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Room = _db.HotelRooms.FirstOrDefault(r => r.Id == model.HotelRoomId);
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            var booking = new Booking
            {
                HotelRoomId = model.HotelRoomId,
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                UserId = user?.Id
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
