# GroceryApiTesting

![Main](https://github.com/Antonio-Kim/GroceryStoreApi/actions/workflows/dotnet.yml/badge.svg)
![.NET Version](https://img.shields.io/badge/.NET-8.0.x-blue)

**Requirement Documentation**: https://github.com/vdespa/Postman-Complete-Guide-API-Testing/blob/main/simple-grocery-store-api.md

There are subtle difference between the APIs:

1. Database implementation instead of In-Memory for Products
2. No PDF export
3. UUID for each products

Requirement API Endpoints have minor errors:

1. Wrong categories on certain items (e.g. Cordless drills are in the fresh produce section)
2. Duplication of Product Id 1709 - Angus Ribeye Steaks

---

### Possible Improvements outside of Requirements

- Enums for Products Categories

#### References

- Data Seeding: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
- Controller Unit Testing: https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
