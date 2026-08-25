using marketplace.api.Infrastructure.Repositories;
using marketplace.api.Infrastructure.Repositories.Interfaces;
using marketplace.api.Service;
using marketplace.api.Service.Interfaces;

namespace marketplace.api.Configuration;
public static class RegistryDependency
{
    public static void ResolveDependencies(this IServiceCollection service)
    {
        service.AddScoped<IProductRepository, ProductRepository>(); 
        service.AddScoped<ISellerProductRepository, SellerProductRepository>();
        service.AddScoped<IProductService, ProductService>();
        service.AddScoped<ISellerProductService, SellerProductService>();
    }
}
