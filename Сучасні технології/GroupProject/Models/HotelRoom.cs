using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class HotelRoom
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва номера обов'язкова!")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, 10000, ErrorMessage = "Ціна має бути від 1 до 10 000")]
        public decimal PricePerNight { get; set; }

        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}