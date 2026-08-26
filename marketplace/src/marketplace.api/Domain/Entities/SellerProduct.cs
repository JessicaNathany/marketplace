namespace marketplace.api.Domain.Entities;

public class SellerProduct
{
    public int Id { get; private set; }
    public string SellerName { get; private set; }
    public int ProductId { get; private set; }
    public string SellerProductId { get; private set; }

    public SellerProduct(string sellerName, int productId, string sellerProductId)
    {
        SellerName = sellerName;
        ProductId = productId;
        SellerProductId = sellerProductId;
    }
}
