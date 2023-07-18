using HotelPrices.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HotelPrices.Models.Context
{
    public class DbHotelPriceContext : DbContext, IDbHotelPriceContext
    {
        protected readonly IConfiguration Configuration;
        public DbHotelPriceContext(DbContextOptions<DbHotelPriceContext> options, IConfiguration configuration
            ) : base(options)
        {
            Configuration = configuration;
        }

        public DbSet<Hotel> Hotel { get; set; }

        public IDbContextTransaction? Transaction { get; private set; }
        public async Task<int> SaveChangesAsync()
        {
            var save = await base.SaveChangesAsync();
            await CommitAsync();
            return save;
        }
        public override int SaveChanges()
        {
            var save = base.SaveChanges();
            Commit();
            return save;
        }
        internal void RollBack()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }
        private void Save()
        {
            try
            {
                ChangeTracker.DetectChanges();
                SaveChanges();
            }
            catch
            {
                RollBack();
                throw;
            }
        }
        private async Task CommitAsync()
        {
            if (Transaction != null)
            {
                await Transaction.CommitAsync();
                await Transaction.DisposeAsync();
                Transaction = null;
            }
        }
        private void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Migrations
            modelBuilder.Entity<Hotel>().HasKey(x => x.Id);
        }
    }
}
