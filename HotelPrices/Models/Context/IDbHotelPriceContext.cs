using HotelPrices.Models.Entites;
using Microsoft.EntityFrameworkCore;

namespace HotelPrices.Models.Context
{
    public interface IDbHotelPriceContext
    {
        DbSet<Hotel> Hotel { get; set; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
