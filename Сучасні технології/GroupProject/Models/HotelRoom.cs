using System.ComponentModel.DataAnnotations;

namespace GroupProject.Models
{
    public class HotelRoom
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Номер кімнати обов'язковий")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Назва номера обов'язкова!")]
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        [Range(1, 10000, ErrorMessage = "Ціна має бути від 1 до 10 000")]
        public decimal PricePerNight { get; set; }

        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;

        public List<Booking> Bookings { get; set; } = default!;
    }
}
