# Marketplace

Marketplace is an API to manage product catalog consolidation from multiple sellers.

## Configuration

### Requirements
- .NET SDK 10
- SQLite (optional, only if you want to inspect DB manually)

### Run locally (multi-platform)
1. Restore dependencies:
   - `dotnet restore`
2. Apply migrations (creates/updates database):
   - `dotnet ef database update --project ./marketplace/src/marketplace.api/marketplace.api.csproj --startup-project ./marketplace/src/marketplace.api/marketplace.api.csproj`
3. Run API:
   - `dotnet run --project ./marketplace/src/marketplace.api/marketplace.api.csproj`

The database file is configured at:
- `marketplace/src/marketplace.api/Infrastructure/Data/catalog.db`

## Import flow

1. Preview imported catalog (from JSON in project):
   - `POST /api/catalog/import?page=1&pageSize=50`
2. Persist catalog into Product and SellerProduct tables:
   - `POST /api/products/import`

## How to test

- `POST /api/catalog/import?page=1&pageSize=50`  
  Reads JSON input, validates/normalizes data, and returns paged preview.

- `POST /api/products/import`  
  Processes catalog items and saves into `Product` and `SellerProduct` with deduplication.

- `GET /api/products?page=1&pageSize=50`  
  Returns paged products.

- `GET /api/products?name=Galaxy&page=1&pageSize=50`  
  Filters products by name.

- `GET /api/products?brand=Samsung&page=1&pageSize=50`  
  Filters products by brand.

- `GET /api/products?category=Electronics&page=1&pageSize=50`  
  Filters products by category.

- `GET /api/products?name=Galaxy&brand=Samsung&category=Electronics&page=1&pageSize=50`  
  Combines filters (name + brand + category).

- `GET /api/seller-product?page=1&pageSize=50`  
  Returns paged seller-product links.

- `GET /api/seller-product/by-seller?sellerName=MegaStore&page=1&pageSize=50`  
  Filters seller-product links by seller name.

## Tests

Run tests with:
- `dotnet test ./marketplace/marketplace.tests/marketplace.tests.csproj`