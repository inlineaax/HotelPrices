namespace HotelPrices.Models.DTO
{
    public class HotelDTO
    {
        public string RoomName { get; set; }
        public int MaxAdults { get; set; }
        public string Condition { get; set; }
        public decimal AveragePricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset CheckIn { get; set; }
        public DateTimeOffset CheckOut { get; set; }
    }
}
