using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelPrices.Models.Entites
{
    [Table("hotel")]
    public class Hotel
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("room_name")]
        public string RoomName { get; set; }

        [Column("max_adults")]
        public int MaxAdults { get; set; }

        [Column("condition")]
        public string Condition { get; set; }

        [Column("price_per_night")]
        public decimal AveragePricePerNight { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("checkin")]
        public DateTimeOffset CheckIn { get; set; }

        [Column("checkout")]
        public DateTimeOffset CheckOut { get; set; }
    }
}
