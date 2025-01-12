using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;
public class MobilesDataContext : AccessoriesDataContext
{
    public MobilesDataContext(DbContextOptions<MobilesDataContext> options) : base(options)
    {
    }
}