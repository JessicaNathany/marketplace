namespace marketplace.api.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Brand { get; private set;  }
    public string Category { get; private set; }

    public ICollection<SellerProduct> SellerProducts { get; private set; } = new List<SellerProduct>();

    public Product(string name, string brand, string category)
    {
        Name = name;
        Brand = brand;
        Category = category;
    }

    public void AddSeller(string sellerName, string sellerProductId)
    {
        if (SellerProducts.Any(x => x.SellerName == sellerName)) return;
        SellerProducts.Add(new SellerProduct(sellerName, Id, sellerProductId));
    }
}
