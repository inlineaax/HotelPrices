using HotelPrices.Models.DTO;

namespace HotelPrices.Models.Services
{
    public interface IWebScraper
    {
        Task<List<HotelDTO>> ScrapeHotelPricesAsync(DateTime checkIn, DateTime checkOut, int adults);
    }
}
