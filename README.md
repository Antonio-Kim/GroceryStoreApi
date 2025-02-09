# GroceryStore API

![main](https://github.com/Antonio-Kim/GroceryStoreApi/actions/workflows/unit-test.yml/badge.svg)
![.NET Version](https://img.shields.io/badge/.NET-8.0.x-blue)

**Requirement Documentation**: https://github.com/vdespa/Postman-Complete-Guide-API-Testing/blob/main/simple-grocery-store-api.md

There are subtle difference between the APIs:

1. Database implementation instead of In-Memory for Products
2. No PDF export
3. UUID for each products
4. ItemId returned in Carts is ProductId instead of a new ID
5. Authentication is random; there's no checking for user that has been registered
6. Returning single Order is not the same response

Requirement API Endpoints have minor errors:

1. Wrong categories on certain items (e.g. Cordless drills are in the fresh produce section)
2. Duplication of Product Id 1709 - Angus Ribeye Steaks

---

### Possible Improvements outside of Requirements

- Enums for Products Categories

#### References

- Data Seeding: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
- Controller Unit Testing: https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
- GitHub Action Workflows: https://docs.github.com/en/actions/using-workflows/events-that-trigger-workflows#workflow_run
