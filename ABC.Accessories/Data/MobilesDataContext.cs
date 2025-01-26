using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;
public class MobilesDataContext(
                        DbContextOptions<MobilesDataContext> options,
                        IConfiguration config
                    ) : AccessoriesDataContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
                    config.GetConnectionString("ABC_Mobiles_DB"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "abc")
                );
    }
}