using ABC.Accessories.Models;
using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;
public class AccessoriesDataContext : DbContext
{
    public AccessoriesDataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Accessory> Accessories => Set<Accessory>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<ItemImage> ItemImages => Set<ItemImage>();
    public DbSet<Seller> Sellers => Set<Seller>();


}
