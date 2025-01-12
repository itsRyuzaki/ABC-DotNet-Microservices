using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;
public class ComputersDataContext : AccessoriesDataContext
{
    public ComputersDataContext(DbContextOptions<ComputersDataContext> options) : base(options)
    {
    }
}