using Bogus;
using Cloud_Mall.Domain.Entities; // Your models' namespace
using Cloud_Mall.Infrastructure.Persistence; // Your DbContext's namespace
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class DataGenerator
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // --- 1. SETUP ---
        // Get the required services
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var faker = new Faker();

        // Ensure the database is clean and freshly created
        //await context.Database.EnsureDeletedAsync();
        //await context.Database.EnsureCreatedAsync();

        // --- 2. SEED INDEPENDENT ENTITIES ---
        // Seed entities that don't depend on others first
        var locationFaker = new Faker<GoverningLocation>()
            .RuleFor(l => l.Name, f => f.Address.State())
            .RuleFor(l => l.Region, f => f.Address.City());
        var locations = locationFaker.Generate(5);
        await context.GoverningLocations.AddRangeAsync(locations);

        var storeCategoryFaker = new Faker<StoreCategory>()
            .RuleFor(sc => sc.Name, f => f.Commerce.Department(1))
            .RuleFor(sc => sc.Description, f => f.Lorem.Sentence());
        var storeCategories = storeCategoryFaker.Generate(8);
        await context.StoreCategories.AddRangeAsync(storeCategories);

        // Save these to the database so they get real IDs
        await context.SaveChangesAsync();

        // --- 3. SEED ROLES ---
        string[] roles = { "Vendor", "Client", "Admin", "Delivery" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // --- 4. SEED USERS AND ASSIGN ROLES ---
        // Create an Admin user
        var adminUser = new ApplicationUser { Name = "Admin User", Email = "admin@cloudmall.com", UserName = "admin@cloudmall.com", CreatedAt = DateTime.UtcNow };
        var adminResult = await userManager.CreateAsync(adminUser, "AdminPassword123!");
        if (adminResult.Succeeded) await userManager.AddToRoleAsync(adminUser, "Admin");

        // Create Vendor users
        var sellers = new List<ApplicationUser>();
        for (int i = 0; i < 15; i++)
        {
            var seller = new ApplicationUser { Name = faker.Name.FullName(), Email = $"vendor{i + 1}@cloudmall.com", UserName = $"vendor{i + 1}@cloudmall.com", CreatedAt = DateTime.UtcNow };
            var result = await userManager.CreateAsync(seller, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(seller, "Vendor");
                sellers.Add(seller);
            }
        }

        // Create Client users
        var clients = new List<ApplicationUser>();
        for (int i = 0; i < 75; i++)
        {
            var client = new ApplicationUser { Name = faker.Name.FullName(), Email = $"client{i + 1}@email.com", UserName = $"client{i + 1}@email.com", CreatedAt = DateTime.UtcNow };
            var result = await userManager.CreateAsync(client, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(client, "Client");
                clients.Add(client);
            }
        }

        // Create Delivery users
        var deliveryUsers = new List<ApplicationUser>();
        for (int i = 0; i < 5; i++)
        {
            var deliveryUser = new ApplicationUser { Name = faker.Name.FullName(), Email = $"delivery{i + 1}@cloudmall.com", UserName = $"delivery{i + 1}@cloudmall.com", CreatedAt = DateTime.UtcNow };
            var result = await userManager.CreateAsync(deliveryUser, "Password123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(deliveryUser, "Delivery");
                deliveryUsers.Add(deliveryUser);
            }
        }

        // --- 5. SEED STORES & ADDRESSES ---
        var stores = new List<Store>();
        var storeFaker = new Faker<Store>()
            .RuleFor(s => s.Name, f => f.Company.CompanyName())
            .RuleFor(s => s.Description, f => f.Company.CatchPhrase())
            .RuleFor(s => s.LogoURL, f => f.Image.PicsumUrl())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past(3))
            .RuleFor(s => s.VendorID, f => f.PickRandom(sellers).Id)
            .RuleFor(s => s.StoreCategoryID, f => f.PickRandom(storeCategories).ID);

        foreach (var seller in sellers)
        {
            var newStores = storeFaker.Generate(faker.Random.Number(1, 3));
            foreach (var store in newStores)
            {
                var addressFaker = new Faker<StoreAddress>()
                    .RuleFor(a => a.StreetAddress, f => f.Address.StreetAddress())
                    .RuleFor(a => a.Notes, f => f.Lorem.Sentence())
                    .RuleFor(a => a.GoverningLocationID, f => f.PickRandom(locations).ID);
                store.Addresses = addressFaker.Generate(faker.Random.Number(1, 2));
            }
            stores.AddRange(newStores);
        }
        await context.Stores.AddRangeAsync(stores);
        await context.SaveChangesAsync();

        // --- 6. SEED PRODUCT CATEGORIES ---
        var productCategories = new List<ProductCategory>();
        foreach (var store in stores)
        {
            var categoryFaker = new Faker<ProductCategory>()
               .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
               .RuleFor(c => c.Description, f => f.Lorem.Sentence())
               .RuleFor(c => c.StoreID, store.ID);
            productCategories.AddRange(categoryFaker.Generate(faker.Random.Number(2, 5)));
        }
        await context.ProductCategories.AddRangeAsync(productCategories);
        await context.SaveChangesAsync();

        // --- 7. SEED PRODUCTS ---
        var products = new List<Product>();
        foreach (var category in productCategories)
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Brand, f => f.Company.CompanyName())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 500)))
                .RuleFor(p => p.Stock, f => f.Random.Number(50, 500))
                .RuleFor(p => p.SKU, f => f.Commerce.Ean13())
                .RuleFor(p => p.ImagesURL, f => f.Image.PicsumUrl())
                .RuleFor(p => p.ProductCategoryID, category.ID)
                .RuleFor(p => p.StoreID, category.StoreID);
            products.AddRange(productFaker.Generate(faker.Random.Number(5, 20)));
        }
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // --- 8. SEED REMAINING ENTITIES (Reviews, Orders, etc.) ---
        var allUsers = sellers.Concat(clients).Concat(deliveryUsers).Append(adminUser).ToList();

        // Seed Reviews
        var reviews = new List<Review>();
        var reviewableProducts = products.Take(150).ToList();
        foreach (var product in reviewableProducts)
        {
            var reviewFaker = new Faker<Review>()
                .RuleFor(r => r.Rate, f => f.Random.Number(1, 5))
                .RuleFor(r => r.Comment, f => f.Rant.Review(product.Name))
                .RuleFor(r => r.CreatedAt, f => f.Date.Past(1))
                .RuleFor(r => r.ClientID, f => f.PickRandom(clients).Id)
                .RuleFor(r => r.ProductID, product.ID);
            reviews.AddRange(reviewFaker.Generate(faker.Random.Number(0, 8)));
        }
        await context.Reviews.AddRangeAsync(reviews);

        // Seed Orders & Order Items
        var orders = new List<Order>();
        foreach (var client in clients)
        {
            var orderFaker = new Faker<Order>()
                .RuleFor(o => o.Status, f => f.PickRandom(new[] { "Pending", "Shipped", "Delivered", "Cancelled" }))
                .RuleFor(o => o.OrderDate, f => f.Date.Past(2))
                .RuleFor(o => o.ShippingCity, f => f.Address.City())
                .RuleFor(o => o.ShippingStreetAddress, f => f.Address.StreetAddress())
                .RuleFor(o => o.ClientID, client.Id);

            var clientOrders = orderFaker.Generate(faker.Random.Number(0, 5));
            foreach (var order in clientOrders)
            {
                var orderItems = new List<OrderItem>();
                var itemsToGenerate = faker.Random.Number(1, 5);
                for (int i = 0; i < itemsToGenerate; i++)
                {
                    var randomProduct = faker.PickRandom(products);
                    var orderItem = new Faker<OrderItem>()
                        .RuleFor(oi => oi.Quantity, f => f.Random.Number(1, 3))
                        .RuleFor(oi => oi.ProductID, randomProduct.ID)
                        .RuleFor(oi => oi.PriceAtTimeOfPurchase, randomProduct.Price);
                    orderItems.Add(orderItem.Generate());
                }
                order.OrderItems = orderItems;
                order.TotalPrice = orderItems.Sum(oi => oi.PriceAtTimeOfPurchase * oi.Quantity);
            }
            orders.AddRange(clientOrders);
        }
        await context.Orders.AddRangeAsync(orders);

        // Seed Carts
        var carts = new List<Cart>();
        foreach (var client in clients)
        {
            carts.Add(new Cart { ClientID = client.Id, UpdatedAt = DateTime.UtcNow });
        }
        await context.Carts.AddRangeAsync(carts);

        // Seed Complaints & Notifications
        var complaintFaker = new Faker<Complaint>()
            .RuleFor(c => c.Title, f => f.Lorem.Word())
            .RuleFor(c => c.Description, f => f.Lorem.Sentences(3))
            .RuleFor(c => c.Status, f => f.PickRandom(new[] { "Open", "In Progress", "Resolved" }))
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(1))
            .RuleFor(c => c.UserID, f => f.PickRandom(allUsers).Id);
        await context.Complaints.AddRangeAsync(complaintFaker.Generate(40));

        var notificationFaker = new Faker<Notification>()
            .RuleFor(n => n.Message, f => f.Lorem.Sentence())
            .RuleFor(n => n.IsRead, f => f.Random.Bool())
            .RuleFor(n => n.CreatedAt, f => f.Date.Past(1))
            .RuleFor(n => n.UserID, f => f.PickRandom(allUsers).Id);
        await context.Notifications.AddRangeAsync(notificationFaker.Generate(100));

        // --- 9. FINAL SAVE ---
        await context.SaveChangesAsync();
    }
}