using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using marketplace.api.Service;

namespace marketplace.tests.Service;

public class CatalogServiceTest
{
    [Fact]
    public async Task ImportCatalogAsync_ShouldNormalizeValidateAndPaginate()
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"catalog-test-{Guid.NewGuid():N}");
        var domainPath = Path.Combine(rootPath, "Domain");
        Directory.CreateDirectory(domainPath);

        var json = """
        [
          { "Id": "id-1", "SellerName": " MegaStore ", "Name": " Smartphone  Galaxy S23 ", "Brand": " Samsung ", "Category": " Electronics " },
          { "Id": "id-2", "SellerName": "TechWorld", "Name": "iPhone 15  Pro", "Brand": null, "Category": "Electronics" },
          { "Id": "", "SellerName": "Invalid", "Name": "Should Be Ignored", "Brand": "X", "Category": "Y" }
        ]
        """;
        await File.WriteAllTextAsync(Path.Combine(domainPath, "import-seller-product.json"), json);

        var environment = new FakeWebHostEnvironment(rootPath);
        var service = new CatalogService(environment, NullLogger<CatalogService>.Instance);

        var result = await service.ImportCatalogAsync(page: 1, pageSize: 1);

        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(1, result.Processed);
        Assert.Equal(1, result.InvalidItems);
        Assert.Equal(1, result.ItemsWithMissingBrand);
        Assert.Single(result.Items);
        Assert.Equal("Smartphone Galaxy S23", result.Items.First().Name);
        Assert.Equal("Samsung", result.Items.First().Brand);
        Assert.Equal("MegaStore", result.Items.First().SellerName);

        Directory.Delete(rootPath, recursive: true);
    }

    [Fact]
    public async Task ImportCatalogAsync_ShouldThrowWhenFileDoesNotExist()
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"catalog-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(rootPath);
        var environment = new FakeWebHostEnvironment(rootPath);
        var service = new CatalogService(environment, NullLogger<CatalogService>.Instance);

        await Assert.ThrowsAsync<FileNotFoundException>(() => service.ImportCatalogAsync(1, 10));

        Directory.Delete(rootPath, recursive: true);
    }

    private sealed class FakeWebHostEnvironment(string contentRootPath) : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "marketplace.tests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = contentRootPath;
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ContentRootPath { get; set; } = contentRootPath;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
