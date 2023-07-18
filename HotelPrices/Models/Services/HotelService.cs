using HotelPrices.Models.DTO;
using HotelPrices.Models.Entites;
using HotelPrices.Models.Repository;
using HotelPrices.Models.Services;

public class HotelService : IHotelService
{
    private readonly IHotelPriceRepository _hotelPriceRepository;

    public HotelService(IHotelPriceRepository hotelPriceRepository) => _hotelPriceRepository = hotelPriceRepository;

    public async Task<List<HotelDTO>> GetHotelPricesAsync(DateTime checkIn, DateTime checkOut)
    {
        var hotels = await _hotelPriceRepository.GetHotelPricesAsync(checkIn, checkOut);

        return hotels.Select(hotel => new HotelDTO
        {
            RoomName = hotel.RoomName,
            MaxAdults = hotel.MaxAdults,
            Condition = hotel.Condition,
            AveragePricePerNight = hotel.AveragePricePerNight,
            TotalPrice = hotel.TotalPrice,
            CheckIn = hotel.CheckIn,
            CheckOut = hotel.CheckOut,
        }).ToList();
    }

    public async Task SaveHotelPricesAsync(List<HotelDTO> hotelPrices)
    {
        var hotels = hotelPrices.Select(hotelDto => new Hotel
        {
            RoomName = hotelDto.RoomName,
            MaxAdults = hotelDto.MaxAdults,
            Condition = hotelDto.Condition,
            AveragePricePerNight = hotelDto.AveragePricePerNight,
            TotalPrice = hotelDto.TotalPrice,
            CheckIn = hotelDto.CheckIn,
            CheckOut = hotelDto.CheckOut,
        }).ToList();

        await _hotelPriceRepository.AddHotelPricesAsync(hotels);
    }
}
