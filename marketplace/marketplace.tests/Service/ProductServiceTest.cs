using marketplace.api.Domain.Dto;
using marketplace.api.Domain.Entities;
using marketplace.api.Infrastructure.Repositories.Interfaces;
using marketplace.api.Service;
using marketplace.api.Service.Interfaces;

namespace marketplace.tests.Service;

public class ProductServiceTest
{
    [Fact]
    public async Task ProcessCatalogAddProduct_ShouldCreateLinkAndIgnoreDuplicates()
    {
        var catalogService = new FakeCatalogService(new CatalogImportResultDto
        {
            TotalItems = 3,
            InvalidItems = 0,
            ItemsWithMissingBrand = 0,
            Items =
            [
                new ImportedSellerProductDto
                {
                    Id = "ext-1",
                    SellerName = "SellerA",
                    Name = "Product 1",
                    Brand = "Brand 1",
                    Category = "Cat 1"
                },
                new ImportedSellerProductDto
                {
                    Id = "ext-2",
                    SellerName = "SellerB",
                    Name = "Product 1",
                    Brand = "Brand 1",
                    Category = "Cat 1"
                },
                new ImportedSellerProductDto
                {
                    Id = "ext-1",
                    SellerName = "SellerA",
                    Name = "Product 1",
                    Brand = "Brand 1",
                    Category = "Cat 1"
                }
            ]
        });

        var productRepository = new FakeProductRepository();
        var sellerProductRepository = new FakeSellerProductRepository();
        var service = new ProductService(catalogService, productRepository, sellerProductRepository);

        var result = await service.ProcessCatalogAddProduct();

        Assert.Equal(3, result.Processed);
        Assert.Equal(1, result.CreatedProducts);
        Assert.Equal(2, result.LinkedSellerProducts);
        Assert.Equal(1, result.IgnoredDuplicates);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldNormalizeFiltersBeforeCallingRepository()
    {
        var catalogService = new FakeCatalogService(new CatalogImportResultDto());
        var productRepository = new FakeProductRepository();
        var sellerProductRepository = new FakeSellerProductRepository();
        var service = new ProductService(catalogService, productRepository, sellerProductRepository);

        await service.GetProductsAsync("  Galaxy  ", "  Samsung ", "   ", 1, 10);

        Assert.Equal("Galaxy", productRepository.LastName);
        Assert.Equal("Samsung", productRepository.LastBrand);
        Assert.Null(productRepository.LastCategory);
        Assert.Equal(1, productRepository.LastPage);
        Assert.Equal(10, productRepository.LastPageSize);
    }

    private sealed class FakeCatalogService(CatalogImportResultDto result) : ICatalogService
    {
        public Task<CatalogImportResultDto> ImportCatalogAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        private readonly List<Product> _products = [];
        private int _nextId = 1;

        public string? LastName { get; private set; }
        public string? LastBrand { get; private set; }
        public string? LastCategory { get; private set; }
        public int LastPage { get; private set; }
        public int LastPageSize { get; private set; }

        public Task<IEnumerable<Product>> GetAllAsync(bool asNoTracking = false)
        {
            return Task.FromResult<IEnumerable<Product>>(_products);
        }

        public Task<Product?> GetByKeyAsync(string name, string brand, string category)
        {
            var item = _products.FirstOrDefault(p => p.Name == name && p.Brand == brand && p.Category == category);
            return Task.FromResult(item);
        }

        public Task<List<Product>> GetByFiltersAsync(string? name, string? brand, string? category, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            LastName = name;
            LastBrand = brand;
            LastCategory = category;
            LastPage = page;
            LastPageSize = pageSize;
            return Task.FromResult(new List<Product>());
        }

        public Task AddAsync(Product product)
        {
            var field = typeof(Product).GetField("<Id>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field!.SetValue(product, _nextId++);
            _products.Add(product);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeSellerProductRepository : ISellerProductRepository
    {
        private readonly HashSet<string> _keys = [];

        public Task<bool> ExistsAsync(string sellerName, string externalId)
        {
            return Task.FromResult(_keys.Contains(BuildKey(sellerName, externalId)));
        }

        public Task AddAsync(SellerProduct sellerProduct)
        {
            _keys.Add(BuildKey(sellerProduct.SellerName, sellerProduct.SellerProductId));
            return Task.CompletedTask;
        }

        public Task<List<SellerProduct>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<SellerProduct>());
        }

        public Task<List<SellerProduct>> GetBySellerNameAsync(string sellerName, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new List<SellerProduct>());
        }

        private static string BuildKey(string sellerName, string externalId)
        {
            return $"{sellerName}::{externalId}";
        }
    }
}
