namespace marketplace.api.Domain.Entities;

public class SellerProduct
{
    public int Id { get; private set; }
    public string SellerName { get; private set; }
    public int ProductId { get; private set; }
    public int SellerProductId { get; private set; }

    public SellerProduct(string sellerName, int productId)
    {
        SellerName = sellerName;
        ProductId = productId;
    }
}
