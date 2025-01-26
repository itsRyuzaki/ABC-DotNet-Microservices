using Microsoft.EntityFrameworkCore;

namespace ABC.Accessories.Data;
public class ComputersDataContext(
                        DbContextOptions<ComputersDataContext> options,
                        IConfiguration config
                    ) : AccessoriesDataContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
                    config.GetConnectionString("ABC_Computers_DB"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "abc")
                );
    }
}