using Bogus;
using Cloud_Mall.Domain.Entities;
using Cloud_Mall.Domain.Enums;
using Cloud_Mall.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class DataGenerator
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // --- 1. SETUP ---
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var faker = new Faker();

        // --- NEW: WIPE OLD DATA BEFORE SEEDING ---
        // This ensures a clean slate on every application start.
        await ClearDataAsync(context);

        // --- 2. DEFINE CONSISTENT CATEGORY STRUCTURE ---
        var categoryMap = new Dictionary<string, List<string>>
        {
            { "Fashion & Apparel", new List<string> { "Men's Clothing", "Women's Clothing", "Shoes & Footwear", "Accessories" } },
            { "Restaurants & Cafes", new List<string> { "Pizza & Pasta", "Burgers & Grills", "Seafood", "Cafes & Desserts" } },
            { "Electronics", new List<string> { "Mobile Phones", "Laptops & Computers", "TVs & Home Theater", "Gadgets" } },
            { "Groceries", new List<string> { "Fresh Produce", "Dairy & Eggs", "Bakery", "Pantry Staples" } },
            { "Home & Furniture", new List<string> { "Living Room Furniture", "Bedroom", "Kitchen & Dining", "Home Decor" } }
        };

        // --- 3. SEED INDEPENDENT ENTITIES (LOOKUP TABLES) ---
        var locationFaker = new Faker<GoverningLocation>()
            .RuleFor(l => l.Name, f => f.Address.State())
            .RuleFor(l => l.Region, f => f.Address.City());
        var locations = locationFaker.Generate(5);
        await context.GoverningLocations.AddRangeAsync(locations);

        var storeCategories = new List<StoreCategory>();
        foreach (var categoryName in categoryMap.Keys)
        {
            storeCategories.Add(new StoreCategory { Name = categoryName, Description = $"Stores related to {categoryName}" });
        }
        await context.StoreCategories.AddRangeAsync(storeCategories);
        await context.SaveChangesAsync(); // Save to get IDs

        // --- 4. SEED ROLES ---
        string[] roles = { "Vendor", "Client", "Admin", "Delivery" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // --- 5. SEED USERS AND ASSIGN ROLES ---
        if (!userManager.Users.Any(u => u.Email == "admin@cloudmall.com"))
        {
            var adminUser = new ApplicationUser { Name = "Admin User", Email = "admin@cloudmall.com", UserName = "admin@cloudmall.com", CreatedAt = DateTime.UtcNow };
            if ((await userManager.CreateAsync(adminUser, "AdminPassword123!")).Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var sellers = new List<ApplicationUser>();
        for (int i = 0; i < 15; i++)
        {
            var email = $"vendor{i + 1}@cloudmall.com";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var seller = new ApplicationUser { Name = faker.Name.FullName(), Email = email, UserName = email, CreatedAt = DateTime.UtcNow };
                if ((await userManager.CreateAsync(seller, "Password123!")).Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, "Vendor");
                    sellers.Add(seller);
                }
            }
        }

        var clients = new List<ApplicationUser>();
        for (int i = 0; i < 75; i++)
        {
            var email = $"client{i + 1}@email.com";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var client = new ApplicationUser { Name = faker.Name.FullName(), Email = email, UserName = email, CreatedAt = DateTime.UtcNow };
                if ((await userManager.CreateAsync(client, "Password123!")).Succeeded)
                {
                    await userManager.AddToRoleAsync(client, "Client");
                    clients.Add(client);
                }
            }
        }

        var deliveryUsers = new List<ApplicationUser>();
        for (int i = 0; i < 5; i++)
        {
            var email = $"delivery{i + 1}@cloudmall.com";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var deliveryUser = new ApplicationUser { Name = faker.Name.FullName(), Email = email, UserName = email, CreatedAt = DateTime.UtcNow };
                if ((await userManager.CreateAsync(deliveryUser, "Password123!")).Succeeded)
                {
                    await userManager.AddToRoleAsync(deliveryUser, "Delivery");
                    deliveryUsers.Add(deliveryUser);
                }
            }
        }

        // Ensure lists are populated with existing users if they weren't created now
        sellers = await userManager.GetUsersInRoleAsync("Vendor") as List<ApplicationUser>;
        clients = await userManager.GetUsersInRoleAsync("Client") as List<ApplicationUser>;
        deliveryUsers = await userManager.GetUsersInRoleAsync("Delivery") as List<ApplicationUser>;

        // --- 6. SEED STORES, ADDRESSES, AND PRODUCT CATEGORIES ---
        var stores = new List<Store>();
        foreach (var seller in sellers)
        {
            // NEW: Assign a random number of stores (2 to 4) to each vendor
            int numberOfStores = faker.Random.Number(2, 4);
            for (int i = 0; i < numberOfStores; i++)
            {
                var selectedStoreCategory = faker.PickRandom(storeCategories);

                var store = new Faker<Store>()
                    .RuleFor(s => s.Name, f => $"{f.Company.CompanyName()} {selectedStoreCategory.Name}")
                    .RuleFor(s => s.Description, f => f.Company.CatchPhrase())
                    .RuleFor(s => s.LogoURL, f => f.Image.PicsumUrl())
                    .RuleFor(s => s.CreatedAt, f => f.Date.Past(3))
                    .RuleFor(s => s.VendorID, seller.Id)
                    .RuleFor(s => s.StoreCategoryID, selectedStoreCategory.ID)
                    .RuleFor(s => s.IsActive, true)
                    .Generate();

                store.Addresses = new Faker<StoreAddress>()
                    .RuleFor(a => a.StreetAddress, f => f.Address.StreetAddress())
                    .RuleFor(a => a.Notes, f => f.Lorem.Sentence())
                    .RuleFor(a => a.GoverningLocationID, f => f.PickRandom(locations).ID)
                    .Generate(faker.Random.Number(1, 2));

                var productCategoryNames = categoryMap[selectedStoreCategory.Name];
                store.ProductCategories = productCategoryNames.Select(pcName => new ProductCategory
                {
                    Name = pcName,
                    Description = $"Products related to {pcName}",
                }).ToList();

                stores.Add(store);
            }
        }
        await context.Stores.AddRangeAsync(stores);
        await context.SaveChangesAsync();

        // --- 7. SEED PRODUCTS ---
        var products = new List<Product>();
        foreach (var store in stores)
        {
            foreach (var category in store.ProductCategories)
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
                    .RuleFor(p => p.StoreID, store.ID);

                products.AddRange(productFaker.Generate(faker.Random.Number(5, 15)));
            }
        }
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // --- 8. SEED DELIVERY COMPANIES ---
        var deliveryCompanies = new List<DeliveryCompany>();
        foreach (var deliveryUser in deliveryUsers)
        {
            deliveryCompanies.Add(new Faker<DeliveryCompany>()
               .RuleFor(dc => dc.Name, f => f.Company.CompanyName() + " Express")
               .RuleFor(dc => dc.CommercialSerialNumber, f => f.Finance.Iban())
               .RuleFor(dc => dc.Phone, f => f.Phone.PhoneNumber())
               .RuleFor(dc => dc.Email, f => f.Internet.Email())
               .RuleFor(dc => dc.CreatedAt, f => f.Date.Past(2))
               .RuleFor(dc => dc.UserID, deliveryUser.Id)
               .Generate());
        }
        await context.DeliveryCompanies.AddRangeAsync(deliveryCompanies);
        await context.SaveChangesAsync();

        // --- 9. SEED ORDERS AND DELIVERY OFFERS ---
        var allProducts = context.Products.ToList();
        var customerOrders = new List<CustomerOrder>();

        foreach (var client in clients)
        {
            int ordersToGenerate = faker.Random.Number(0, 4);
            for (int i = 0; i < ordersToGenerate; i++)
            {
                // ... (Order creation logic from before, slightly adjusted)
                // The rest of the seeding logic continues here for orders, reviews, etc.
                var customerOrder = new CustomerOrder
                {
                    ClientID = client.Id,
                    OrderDate = faker.Date.Past(1)
                };

                var itemsInCart = faker.PickRandom(allProducts, faker.Random.Number(1, 6)).ToList();
                var itemsGroupedByStore = itemsInCart.GroupBy(p => p.StoreID);

                foreach (var storeGroup in itemsGroupedByStore)
                {
                    var vendorOrder = new VendorOrder
                    {
                        StoreID = storeGroup.Key,
                        OrderDate = customerOrder.OrderDate,
                        ShippingCity = faker.Address.City(),
                        ShippingStreetAddress = faker.Address.StreetAddress(),
                        Status = faker.PickRandom<VendorOrderStatus>()
                    };

                    vendorOrder.OrderItems = storeGroup.Select(product => new OrderItem
                    {
                        ProductID = product.ID,
                        Quantity = faker.Random.Number(1, 3),
                        PriceAtTimeOfPurchase = product.Price
                    }).ToList();

                    vendorOrder.SubTotal = vendorOrder.OrderItems.Sum(oi => oi.PriceAtTimeOfPurchase * oi.Quantity);
                    customerOrder.VendorOrders.Add(vendorOrder);
                }
                customerOrder.GrandTotal = customerOrder.VendorOrders.Sum(vo => vo.SubTotal);

                // Add Delivery Offers
                if (customerOrder.VendorOrders.Any())
                {
                    customerOrder.DeliveryOffers = new Faker<DeliveryOffer>()
                        .RuleFor(o => o.Price, f => f.Finance.Amount(5, 50))
                        .RuleFor(o => o.EstimatedDays, f => f.Random.Number(1, 7))
                        .RuleFor(o => o.OfferDate, f => customerOrder.OrderDate)
                        .RuleFor(o => o.Status, f => f.PickRandom<DeliveryOfferStatus>())
                        .RuleFor(o => o.DeliveryCompanyID, f => f.PickRandom(deliveryCompanies).ID)
                        .Generate(faker.Random.Number(1, 3));

                    customerOrders.Add(customerOrder);
                }
            }
        }
        await context.CustomerOrders.AddRangeAsync(customerOrders);

        // --- 10. SEED CARTS, REVIEWS, AND OTHER ENTITIES ---
        var allUsers = userManager.Users.ToList();

        var reviews = new List<Review>();
        foreach (var product in allProducts.Take(200))
        {
            reviews.AddRange(new Faker<Review>()
                .RuleFor(r => r.Rate, f => f.Random.Number(1, 5))
                .RuleFor(r => r.Comment, f => f.Rant.Review(product.Name))
                .RuleFor(r => r.CreatedAt, f => f.Date.Past(1))
                .RuleFor(r => r.ClientID, f => f.PickRandom(clients).Id)
                .RuleFor(r => r.ProductID, product.ID)
                .Generate(faker.Random.Number(0, 5)));
        }
        await context.Reviews.AddRangeAsync(reviews);

        var carts = new List<Cart>();
        foreach (var client in clients)
        {
            carts.Add(new Cart { ClientID = client.Id, UpdatedAt = DateTime.UtcNow });
        }
        await context.Carts.AddRangeAsync(carts);

        var complaints = new Faker<Complaint>()
            .RuleFor(c => c.Title, f => f.Lorem.Word())
            .RuleFor(c => c.Description, f => f.Lorem.Sentences(3))
            .RuleFor(c => c.Status, f => f.PickRandom(new[] { "Open", "In Progress", "Resolved" }))
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(1))
            .RuleFor(c => c.UserID, f => f.PickRandom(allUsers).Id)
            .Generate(40);
        await context.Complaints.AddRangeAsync(complaints);

        var notifications = new Faker<Notification>()
            .RuleFor(n => n.Message, f => f.Lorem.Sentence())
            .RuleFor(n => n.IsRead, f => f.Random.Bool())
            .RuleFor(n => n.CreatedAt, f => f.Date.Past(1))
            .RuleFor(n => n.UserID, f => f.PickRandom(allUsers).Id)
            .Generate(100);
        await context.Notifications.AddRangeAsync(notifications);

        // --- 11. FINAL SAVE ---
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Clears all business-related data from the database, respecting foreign key constraints.
    /// Does NOT delete Users or Roles.
    /// </summary>
    private static async Task ClearDataAsync(ApplicationDbContext context)
    {
        // Deletion must happen in a specific order to avoid FK constraint violations.
        // Start with entities that have the most dependencies on others.
        context.DeliveryOffers.RemoveRange(context.DeliveryOffers);
        context.OrderItems.RemoveRange(context.OrderItems);
        context.Reviews.RemoveRange(context.Reviews);
        context.CartItems.RemoveRange(context.CartItems);
        context.Complaints.RemoveRange(context.Complaints);
        context.Notifications.RemoveRange(context.Notifications);
        await context.SaveChangesAsync();

        context.VendorOrders.RemoveRange(context.VendorOrders);
        context.Products.RemoveRange(context.Products);
        await context.SaveChangesAsync();

        context.ProductCategories.RemoveRange(context.ProductCategories);
        context.StoreAddresses.RemoveRange(context.StoreAddresses);
        context.DeliveryCompanies.RemoveRange(context.DeliveryCompanies);
        context.CustomerOrders.RemoveRange(context.CustomerOrders);
        await context.SaveChangesAsync();

        context.Stores.RemoveRange(context.Stores);
        context.Carts.RemoveRange(context.Carts);
        await context.SaveChangesAsync();

        context.StoreCategories.RemoveRange(context.StoreCategories);
        context.GoverningLocations.RemoveRange(context.GoverningLocations);

        await context.SaveChangesAsync();
    }
}