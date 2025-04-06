using System.ComponentModel.DataAnnotations;
using GroupProject.Models;

namespace GroupProject.ViewModels
{
    public class RoomBookingViewModel
    {
        [Required]
        public int HotelRoomId { get; set; }

        public HotelRoom? Room { get; set; }

        [Required(ErrorMessage = "Ім'я гостя обов'язкове!")]
        public string GuestName { get; set; } = default!;

        [Required(ErrorMessage = "Email гостя обов'язковий!")]
        [EmailAddress(ErrorMessage = "Некоректний email!")]
        public string GuestEmail { get; set; } = default!;

        [Required(ErrorMessage = "Дата заїзду обов'язкова!")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Дата виїзду обов'язкова!")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
    }
}
