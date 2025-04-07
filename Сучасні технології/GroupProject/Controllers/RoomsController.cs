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

        public async Task<IActionResult> Index(int? pageIndex)
        {
            const int pageSize = 5; // Кількість кімнат на сторінці

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
