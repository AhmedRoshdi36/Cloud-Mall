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

        // Check if data already exists to prevent re-seeding
        if (context.Users.Any())
        {
            return; // DB has been seeded
        }

        // --- 2. SEED INDEPENDENT ENTITIES ---
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
        var adminUser = new ApplicationUser { Name = "Admin User", Email = "admin@cloudmall.com", UserName = "admin@cloudmall.com", CreatedAt = DateTime.UtcNow };
        if ((await userManager.CreateAsync(adminUser, "AdminPassword123!")).Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");

        var sellers = new List<ApplicationUser>();
        for (int i = 0; i < 15; i++)
        {
            var seller = new ApplicationUser { Name = faker.Name.FullName(), Email = $"vendor{i + 1}@cloudmall.com", UserName = $"vendor{i + 1}@cloudmall.com", CreatedAt = DateTime.UtcNow };
            if ((await userManager.CreateAsync(seller, "Password123!")).Succeeded)
            {
                await userManager.AddToRoleAsync(seller, "Vendor");
                sellers.Add(seller);
            }
        }

        var clients = new List<ApplicationUser>();
        for (int i = 0; i < 75; i++)
        {
            var client = new ApplicationUser { Name = faker.Name.FullName(), Email = $"client{i + 1}@email.com", UserName = $"client{i + 1}@email.com", CreatedAt = DateTime.UtcNow };
            if ((await userManager.CreateAsync(client, "Password123!")).Succeeded)
            {
                await userManager.AddToRoleAsync(client, "Client");
                clients.Add(client);
            }
        }

        var deliveryUsers = new List<ApplicationUser>();
        for (int i = 0; i < 5; i++)
        {
            var deliveryUser = new ApplicationUser { Name = faker.Name.FullName(), Email = $"delivery{i + 1}@cloudmall.com", UserName = $"delivery{i + 1}@cloudmall.com", CreatedAt = DateTime.UtcNow };
            if ((await userManager.CreateAsync(deliveryUser, "Password123!")).Succeeded)
            {
                await userManager.AddToRoleAsync(deliveryUser, "Delivery");
                deliveryUsers.Add(deliveryUser);
            }
        }

        // --- 5. SEED STORES, ADDRESSES, AND PRODUCT CATEGORIES ---
        var stores = new List<Store>();
        foreach (var seller in sellers)
        {
            var newStores = new Faker<Store>()
                .RuleFor(s => s.Name, f => f.Company.CompanyName())
                .RuleFor(s => s.Description, f => f.Company.CatchPhrase())
                .RuleFor(s => s.LogoURL, f => f.Image.PicsumUrl())
                .RuleFor(s => s.CreatedAt, f => f.Date.Past(3))
                .RuleFor(s => s.VendorID, seller.Id)
                .RuleFor(s => s.StoreCategoryID, f => f.PickRandom(storeCategories).ID)
                .Generate(faker.Random.Number(1, 2));

            foreach (var store in newStores)
            {
                store.Addresses = new Faker<StoreAddress>()
                    .RuleFor(a => a.StreetAddress, f => f.Address.StreetAddress())
                    .RuleFor(a => a.Notes, f => f.Lorem.Sentence())
                    .RuleFor(a => a.GoverningLocationID, f => f.PickRandom(locations).ID)
                    .Generate(faker.Random.Number(1, 2));

                store.ProductCategories = new Faker<ProductCategory>()
                   .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                   .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                   .RuleFor(c => c.StoreID, store.ID)
                   .Generate(faker.Random.Number(2, 5));
            }
            stores.AddRange(newStores);
        }
        await context.Stores.AddRangeAsync(stores);
        await context.SaveChangesAsync();


        // --- 7. SEED PRODUCTS ---
        var products = new List<Product>();
        var allProductCategories = context.ProductCategories.ToList();
        foreach (var category in allProductCategories)
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

            products.AddRange(productFaker.Generate(faker.Random.Number(5, 15)));
        }
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();


        // --- 8. SEED ORDERS ---
        var allProducts = context.Products.ToList();
        var customerOrders = new List<CustomerOrder>();

        foreach (var client in clients)
        {
            var checkoutsToGenerate = faker.Random.Number(0, 4);
            for (int i = 0; i < checkoutsToGenerate; i++)
            {
                var customerOrder = new CustomerOrder
                {
                    ClientID = client.Id,
                    OrderDate = faker.Date.Past(1)
                };

                var itemsInCart = new List<Product>();
                var cartItemCount = faker.Random.Number(1, 6);
                for (int j = 0; j < cartItemCount; j++)
                {
                    itemsInCart.Add(faker.PickRandom(allProducts));
                }

                var itemsGroupedByStore = itemsInCart.GroupBy(p => p.StoreID);

                foreach (var storeGroup in itemsGroupedByStore)
                {
                    var vendorOrder = new VendorOrder
                    {
                        CustomerOrder = customerOrder,
                        StoreID = storeGroup.Key,
                        OrderDate = customerOrder.OrderDate,
                        ShippingCity = faker.Address.City(),
                        ShippingStreetAddress = faker.Address.StreetAddress(),
                        Status = faker.PickRandom<VendorOrderStatus>()
                    };

                    var orderItems = new List<OrderItem>();
                    foreach (var product in storeGroup)
                    {
                        orderItems.Add(new OrderItem
                        {
                            VendorOrder = vendorOrder,
                            ProductID = product.ID,
                            Quantity = faker.Random.Number(1, 3),
                            PriceAtTimeOfPurchase = product.Price
                        });
                    }

                    vendorOrder.OrderItems = orderItems;
                    vendorOrder.SubTotal = orderItems.Sum(oi => oi.PriceAtTimeOfPurchase * oi.Quantity);
                    customerOrder.VendorOrders.Add(vendorOrder);
                }
                customerOrder.GrandTotal = customerOrder.VendorOrders.Sum(vo => vo.SubTotal);

                if (customerOrder.VendorOrders.Any())
                {
                    customerOrders.Add(customerOrder);
                }
            }
        }
        await context.CustomerOrders.AddRangeAsync(customerOrders);

        // --- 9. SEED REMAINING ENTITIES (Reviews, Carts, etc.) ---
        var allUsers = sellers.Concat(clients).Concat(deliveryUsers).Append(adminUser).ToList();

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

        // --- 10. FINAL SAVE ---
        await context.SaveChangesAsync();
    }
}