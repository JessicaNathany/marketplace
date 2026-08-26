using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace marketplace.api.Infrastructure.Data;

public class DesignTimeMarketplaceDbContextFactory : IDesignTimeDbContextFactory<MarketplaceDbContext>
{
    public MarketplaceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MarketplaceDbContext>();
        optionsBuilder.UseSqlite("Data Source=Infrastructure/Data/catalog.db");

        return new MarketplaceDbContext(optionsBuilder.Options);
    }
}
