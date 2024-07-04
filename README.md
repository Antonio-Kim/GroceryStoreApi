# GroceryApiTesting

**Requirement Documentation**: https://github.com/vdespa/Postman-Complete-Guide-API-Testing/blob/main/simple-grocery-store-api.md

There are subtle difference between the APIs:

1. HATEOAS compliant
2. Database implementation instead of In-Memory for Products
3. No PDF export
4. No underscore for current_stock column
5. Single Id for each products

Requirement API Endpoints have minor errors:

1. Wrong categories on certain items (e.g. Cordless drills are in the fresh produce section)
2. Duplication of Product Id 1709 - Angus Ribeye Steaks

---

### Possible Improvements outside of Requirements

- Enums for Products Categories

#### References

- Data Seeding: https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding
