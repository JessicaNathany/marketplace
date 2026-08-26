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
<img width="926" height="621" alt="image" src="https://github.com/user-attachments/assets/afc08d38-0669-486d-b6ea-6d22d46d6378" />

<img width="963" height="632" alt="image" src="https://github.com/user-attachments/assets/6af909fe-cbc4-437b-b30d-2625d01d6b77" />

<img width="826" height="668" alt="image" src="https://github.com/user-attachments/assets/ce2a0e03-5954-4d54-b187-c5f0d511207e" />


