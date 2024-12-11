using ABC.Accessories.Models;
using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data
{
    public class AccessoriesDataContext : DbContext
    {
        public AccessoriesDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Accessory> Mobiles => Set<Accessory>();
    }
}