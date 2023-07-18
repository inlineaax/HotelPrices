using HotelPrices.Models.Entites;

namespace HotelPrices.Models.Repository
{
    public interface IHotelPriceRepository
    {
        Task<List<Hotel>> GetHotelPricesAsync(DateTime checkIn, DateTime checkOut);
        Task AddHotelPricesAsync(List<Hotel> hotelPrices);
    }
}
