using System.ComponentModel.DataAnnotations;
using GroupProject.Models;

namespace GroupProject.ViewModels
{
    public class RoomBookingViewModel
    {
        public HotelRoom Room { get; set; } = default!;

        [Required(ErrorMessage = "Ім'я гостя обов'язкове!")]
        public string GuestName { get; set; } = default!;

        [Required(ErrorMessage = "Email гостя обов'язковий!")]
        [EmailAddress(ErrorMessage = "Некоректний email!")]
        public string GuestEmail { get; set; } = default!;

        [Required(ErrorMessage = "Дата заїзду обов'язкова!")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Дата виїзду обов'язкова!")]
        public DateTime CheckOutDate { get; set; }
    }
}
