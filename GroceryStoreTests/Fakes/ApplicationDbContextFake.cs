using GroceryStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreTests.Fakes;

public class ApplicationDbContextFake : ApplicationDbContext
{
    public ApplicationDbContextFake() : base(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName:
            $"GroceryStore-{Guid.NewGuid()}")
        .Options)
    { }

}