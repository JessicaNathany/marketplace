namespace marketplace.api.Domain.Entities;

public class Product
{
    private readonly List<SellerProduct> _seller = new();

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Brand { get; private set;  }
    public string Category { get; private set; }

    public IReadOnlyCollection<SellerProduct> SellerProducts => _seller.AsReadOnly();

    public Product(string name, string brand, string category)
    {
        Name = name;
        Brand = brand;
        Category = category;
    }

    public void AddSeller(string sellerName)
    {
        if (_seller.Any(x => x.SellerName == sellerName)) return;
        _seller.Add(new SellerProduct(sellerName, Id));
    }
}
