using HotelPrices.Models.Context;
using HotelPrices.Models.Entites;
using HotelPrices.Models.Repository;
using Microsoft.EntityFrameworkCore;

public class HotelPriceRepository : IHotelPriceRepository
{
    private readonly IDbHotelPriceContext _context;

    public HotelPriceRepository(IDbHotelPriceContext context)
    {
        _context = context;
    }

    public async Task<List<Hotel>> GetHotelPricesAsync(DateTime checkIn, DateTime checkOut)
    {
        // Converter as datas DateTime para DateTimeOffset para fazer a comparação no banco de dados
        DateTimeOffset checkInDateTimeOffset = new DateTimeOffset(checkIn, TimeSpan.Zero);
        DateTimeOffset checkOutDateTimeOffset = new DateTimeOffset(checkOut, TimeSpan.Zero);

        return await _context.Hotel
        .Where(h => h.CheckIn == checkInDateTimeOffset && h.CheckOut == checkOutDateTimeOffset)
        .ToListAsync();
    }

    public async Task AddHotelPricesAsync(List<Hotel> hotelPrices)
    {
        _context.Hotel.AddRange(hotelPrices);
        await _context.SaveChangesAsync();
    }
}
