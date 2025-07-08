# Cloud Mall API: Full Project Documentation & Analysis

![.NET](https://img.shields.io/badge/.NET-8-blue.svg) ![C#](https://img.shields.io/badge/C%23-12-purple.svg) ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blueviolet.svg) ![Architecture](https://img.shields.io/badge/Architecture-Clean%20%7C%20CQRS-red)

This document provides a complete overview of the **Cloud Mall API**, the backend engine for a sophisticated multi-vendor e-commerce platform. It covers the business model, technical architecture, setup instructions, and future roadmap.

---

## 1. Project Vision & Business Analysis 📈

### 1.1. The Business Idea

**Cloud Mall** is a multi-vendor e-commerce platform designed to empower small to medium-sized businesses (SMBs), artisans, and independent retailers to establish a robust online presence. It provides a centralized marketplace where vendors can create their own digital storefronts, manage products, and reach a wider audience, while customers enjoy a seamless and diverse shopping experience from a single portal.

### 1.2. Value Proposition

- **For Vendors**: Cloud Mall offers a turn-key, low-risk solution to enter the e-commerce space without the high cost and technical complexity of building and maintaining a standalone website. It provides intuitive tools for product management, inventory tracking, order fulfillment, and visibility within a larger, active marketplace.
- **For Clients**: The platform offers a rich, centralized shopping destination with a wide variety of products from different sellers. A unified cart and checkout process simplifies purchasing from multiple stores at once.

### 1.3. Target Audience

- **Clients (Shoppers)**: Online shoppers looking for unique products, reliable service, and a convenient, secure shopping experience across various stores.
- **Vendors (Sellers)**: Small business owners, independent creators, and local retailers who need a simple yet powerful platform to sell their goods online. They may have limited technical expertise and budget.
- **Platform Administrators**: The internal Cloud Mall team responsible for overseeing platform operations, managing users, resolving disputes, and ensuring a healthy marketplace ecosystem.
- **Delivery Personnel**: Individuals or services responsible for the logistics of delivering orders from vendors to clients.

### 1.4. Monetization Strategy

The primary business model for Cloud Mall is **commission-based**, ensuring a partnership where the platform succeeds when its vendors succeed.

- **Transaction Fees**: The platform will take a competitive percentage (e.g., 5-10%) of every sale made through the marketplace. This is a low-risk model for vendors, as they only pay when they make a sale.
- **Future Monetization Channels**:
  - **Vendor Subscriptions**: A premium tier for vendors (e.g., "Vendor Pro") could offer benefits like lower transaction fees, advanced sales analytics, and premium store customization options.
  - **Featured Listings**: Vendors could pay to have their products or stores promoted on the homepage or at the top of search and category results.
  - **Logistics Services**: Offer an integrated delivery service for an additional fee, leveraging the `Delivery` user role.

### 1.5. Payment Processing

To handle transactions securely and efficiently, the platform is designed to integrate with a third-party payment gateway that specializes in marketplaces.

- **Proposed Gateway**: **Stripe Connect** is highly recommended. It's built for platforms like Cloud Mall and handles the complex flow of paying out multiple vendors from a single customer transaction while automatically managing commission splits.
- **Payment Flow**:
  1.  A Client pays for items from multiple vendors in a single transaction.
  2.  The full payment is processed by Stripe.
  3.  Stripe automatically deducts the platform's commission.
  4.  The remaining funds are routed directly to the respective vendors' connected accounts.
      This automates the payment lifecycle and minimizes financial overhead and liability for the platform.

---

## 2. Core Features & API Endpoints 🚀

The API provides a comprehensive set of features to power the marketplace.

- **User Management**: Secure, role-based registration and JWT authentication for `Admin`, `Vendor`, `Client`, and `Delivery` roles.
- **Store Management**: Vendors can create and manage their own storefronts, including logos and addresses. Admins can enable or disable stores.
- **Product Management**: Vendors can add, update, and manage products within their stores. Admins can moderate and remove products.
- **Advanced Browse**: Clients can browse stores and products with powerful filtering (by name, brand, price, category, rating) and server-side pagination.
- **Full Shopping Cart**: Persistent shopping cart per client. Add products, update quantities, and remove items, and view cart totals.
- **Multi-Vendor Order System**: A sophisticated checkout process that splits a single cart into multiple `VendorOrder`s, one for each store, all grouped under a single `CustomerOrder`.
- **Order Fulfillment**: Vendors can view their orders and update the fulfillment status (e.g., Pending, Shipped, Fulfilled).
- **Ratings and Reviews**: Clients can leave reviews and ratings for products they have purchased.
- **Data Seeding**: Comes with a rich data generator using `Bogus` to populate the database with realistic fake data for development and testing.

---

## 3. Technology Stack 🛠️

- **Backend Framework**: **ASP.NET Core 8**
- **Language**: **C# 12**
- **Database**: **Microsoft SQL Server**
- **ORM**: **Entity Framework Core 8**
- **Key Architectural Libraries**:
  - **MediatR**: For implementing the CQRS and Mediator patterns, decoupling business logic handlers.
  - **FluentValidation**: For creating clean, powerful, and decoupled validation rules.
  - **AutoMapper**: For simplifying the mapping between domain entities and Data Transfer Objects (DTOs).
- **Authentication**: **ASP.NET Core Identity** for user management, secured by **JWT (JSON Web Tokens)**.
- **Development Tools**:
  - **Bogus**: For generating realistic seed data.
  - **Swagger/OpenAPI**: For API documentation and interactive testing.

---

## 4. Architectural Overview & Philosophy 🧠

The architecture was deliberately chosen to create a system that is maintainable, testable, and scalable, following industry best practices.

### 4.1. Clean Architecture

- **What it is**: A design philosophy that separates the software into concentric layers: **Domain**, **Application**, **Infrastructure**, and **Presentation (API)**. The core principle is the **Dependency Rule**: source code dependencies can only point inwards.
- **Why we used it**:
  - **Testability**: The core business logic (Application and Domain layers) is completely independent of external frameworks like EF Core or ASP.NET Core. This allows for simple, fast, and reliable unit testing.
  - **Maintainability**: Changes to external details (e.g., swapping the database from SQL Server to PostgreSQL, or adding a gRPC interface alongside the REST API) have minimal impact on the core logic, making the system easier to evolve.
  - **Separation of Concerns**: Each layer has a single, clear responsibility, making the codebase easier for developers to understand and manage.

### 4.2. CQRS with MediatR

- **What it is**: **Command Query Responsibility Segregation** is a pattern that separates read operations (**Queries**) from write operations (**Commands**). `MediatR` is a library that helps implement this by dispatching a request object to a single handler.
- **Why we used it**:
  - **Simplicity & Focus**: It simplifies models. A query can be optimized to return a DTO shaped exactly as the UI needs it, while a command can be tailored specifically for validation and processing. Each handler has one, and only one, job.
  - **Performance**: Read and write operations can be optimized independently. For example, read queries can bypass the Unit of Work for performance, while write commands use it to ensure transactional integrity.

### 4.3. Repository and Unit of Work Patterns

- **What they are**: The **Repository Pattern** abstracts data access logic behind an interface (e.g., `IProductRepository`). The **Unit of Work Pattern** (`IUnitOfWork`) groups multiple repository operations into a single atomic transaction.
- **Why we used them**:
  - **Decoupling**: They completely decouple the Application Layer from the data access technology (Entity Framework Core). The business logic doesn't know or care how data is being stored.
  - **Transactional Integrity**: The Unit of Work ensures that all changes within a single business operation (like creating an order) are saved together or not at all, preventing data corruption.

---

## 5. Project Structure

/
├── Cloud_Mall.API/ # Presentation Layer (ASP.NET Core Web API)
│ ├── Controllers/
│ └── Program.cs
│
├── Cloud_Mall.Application/ # Application Layer (Business Logic)
│ ├── Behaviors/ # MediatR pipeline behaviors (e.g., Validation)
│ ├── DTOs/ # Data Transfer Objects
│ ├── Interfaces/ # Abstractions for Infrastructure (IRepository, IFileService)
│ └── Features/ # Use cases organized by feature (e.g., Products, Orders)
│
├── Cloud_Mall.Domain/ # Domain Layer (Core Business Entities & Enums)
│ ├── Entities/
│ └── Enums/
│
└── Cloud_Mall.Infrastructure/ # Infrastructure Layer (External Concerns)
├── Persistence/ # EF Core (DbContext, Migrations, Repositories)
└── Services/ # Concrete implementations (FileService, JwtTokenGenerator)

---

## 6. Local Setup & Installation ⚙️

1.  **Prerequisites**:

    - .NET 8 SDK
    - SQL Server (LocalDB, Express, or Developer edition)

2.  **Clone the Repository**:

    ```bash
    git clone <your-repository-url>
    cd <repository-folder>
    ```

3.  **Configure Database Connection**:

    - Open `src/Cloud_Mall.API/appsettings.Development.json`.
    - Modify the `conn1` connection string to point to your local SQL Server instance.

4.  **Configure Secrets**:

    - For security, the JWT secret key **must** be stored outside of version control using the .NET Secret Manager.
    - Navigate to the API project directory in your terminal: `cd src/Cloud_Mall.API`
    - Initialize user secrets: `dotnet user-secrets init`
    - Set the secret key: `dotnet user-secrets set "JwtSettings:Secret" "YOUR_SUPER_SECRET_AND_LONG_KEY_THAT_IS_AT_LEAST_32_BYTES_LONG"`

5.  **Apply Database Migrations**:

    - Run the following command from the root directory of the solution:

    ```bash
    dotnet ef database update --project src/Cloud_Mall.Infrastructure --startup-project src/Cloud_Mall.API
    ```

6.  **Run the Application**:
    - Run from the root directory:
    ```bash
    dotnet run --project src/Cloud_Mall.API
    ```
    - Navigate to `https://localhost:PORT/swagger` in your browser to explore and test the API endpoints.

---

## 7. Key Recommendations & Future Roadmap ✅

Based on a full code review, the following are the highest-priority recommendations for future development:

- **High Priority - Performance**:

  - **Fix In-Memory Pagination**: The `StoreRepository` currently fetches all stores before paginating in memory. This must be refactored to perform pagination at the database level (using `.Skip().Take()` on the `IQueryable`) to ensure scalability.

- **High Priority - Security**:

  - **Secure Vendor Endpoints**: Add ownership checks to queries like `GetSingleProductForVendorQuery` to ensure a vendor cannot access resources belonging to another vendor.

- **Medium Priority - Production Readiness**:

  - **Cloud File Storage**: Implement an `IFileService` for a cloud provider like Azure Blob Storage or Amazon S3. The current local file system implementation will not work in a load-balanced cloud environment.
  - **Implement Payment Gateway**: Integrate **Stripe Connect** to handle all financial transactions, commissions, and vendor payouts.

- **Low Priority - Code Quality & Consistency**:
  - **Use Enums for Statuses**: Convert the `string` status field in the `Complaint` entity to a strongly-typed `enum` for better type safety.
  - **Refactor Cart Handlers**: Change cart command handlers (`Add`, `Update`, `Remove`) to return `ApiResponse<T>` instead of a simple `bool` to provide more meaningful error feedback to the client.
  - **Complete Missing Features**: Implement the `DeleteStoreCommandHandler` and other unfinished parts of the API.
