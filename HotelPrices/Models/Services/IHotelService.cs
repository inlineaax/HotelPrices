using HotelPrices.Models.DTO;

namespace HotelPrices.Models.Services
{
    public interface IHotelService
    {
        Task<List<HotelDTO>> GetHotelPricesAsync(DateTime checkIn, DateTime checkOut);
        Task SaveHotelPricesAsync(List<HotelDTO> hotelPrices);
    }
}
