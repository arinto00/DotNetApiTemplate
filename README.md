# .NET Clean Architecture API Template

A production-ready template for building modern, maintainable .NET APIs following Clean Architecture principles. This template provides a solid foundation for creating scalable microservices or backend APIs.

[![Build Status](https://github.com/yourusername/DotNetApiTemplate/workflows/Build%20and%20Test/badge.svg)](https://github.com/yourusername/DotNetApiTemplate/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 🔥 Features

- **Clean Architecture** implementation with proper separation of concerns
- **Domain-Driven Design** principles
- **Repository Pattern** for data access abstraction
- **CQRS-like** approach using services and repositories
- **Swagger/OpenAPI** documentation
- **JWT Authentication** configured and ready to use
- **Health Checks** for service monitoring
- **Entity Framework Core** with proper configurations
- **Global Exception Handling** middleware
- **Dependency Injection** configured for all layers
- **Automated Auditing** for entities (created/modified)
- **Docker Support** with multi-stage builds
- **GitHub Actions** workflow included
- **Unit Tests** with xUnit and Moq

## 🏗️ Architecture Overview

This template follows Clean Architecture principles with 4 main layers:

![Architecture Diagram](https://raw.githubusercontent.com/yourusername/DotNetApiTemplate/main/docs/images/architecture-diagram.png)

1. **Domain Layer**: The core of the application containing business entities, enums, exceptions, and interfaces
2. **Application Layer**: Contains business logic, interfaces for infrastructure, services, and DTOs
3. **Infrastructure Layer**: Implements interfaces defined in the application layer (database, identity, etc.)
4. **API Layer**: Handles HTTP requests, routes, controllers, and configuration

## 🚀 Getting Started

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download) or later
- A code editor (Visual Studio, VS Code, JetBrains Rider, etc.)
- Git

### Using This Template

#### Method 1: Create from GitHub Template

1. Click the "Use this template" button at the top of the repository
2. Name your new repository
3. Clone your new repository locally:
   ```bash
   git clone https://github.com/yourusername/your-new-repo.git
   cd your-new-repo
   ```

#### Method 2: Clone and Customize

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/DotNetApiTemplate.git
   cd DotNetApiTemplate
   ```

2. Rename the solution and projects:
   ```bash
   # Example renaming for a Product API
   find . -type f -name "*.sln" -o -name "*.csproj" -o -name "*.cs" | xargs sed -i '' 's/ApiTemplate/ProductApi/g'
   find . -type f -name "*.sln" -o -name "*.csproj" -o -name "*.cs" | xargs sed -i '' 's/apitemplate/productapi/g'
   ```

3. Rename directories and files:
   ```bash
   # Rename directories
   find . -depth -type d -name "*ApiTemplate*" -exec sh -c 'mv "$0" "${0//ApiTemplate/ProductApi}"' {} \;
   
   # Rename files
   find . -type f -name "*ApiTemplate*" -exec sh -c 'mv "$0" "${0//ApiTemplate/ProductApi}"' {} \;
   ```

### Database Setup

This template can work with different database providers:

#### For SQL Server:

Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=YourDatabase;User=your-user;Password=your-password;TrustServerCertificate=True;"
}
```

#### For SQLite (Mac/Linux Development):

1. Update Infrastructure/DependencyInjection.cs:
```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
```

2. Update appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YourDatabase.db"
}
```

### Running the API

```bash
cd src/YourProject.API
dotnet run
```

The API will be available at:
- https://localhost:7001 (HTTPS) 
- http://localhost:5000 (HTTP)

Swagger documentation is accessible at the root URL.

## 📁 Project Structure

```
ApiTemplate/
├── src/
│   ├── ApiTemplate.API/           # API layer with controllers and config
│   │   ├── Controllers/           # API controllers
│   │   ├── Middleware/            # Custom middleware
│   │   ├── Extensions/            # Extension methods
│   │   └── Program.cs             # Application entry point
│   │
│   ├── ApiTemplate.Application/   # Application layer with business logic
│   │   ├── Common/                # Shared components
│   │   ├── Interfaces/            # Service interfaces
│   │   ├── Services/              # Service implementations
│   │   └── Mapping/               # AutoMapper profiles
│   │
│   ├── ApiTemplate.Domain/        # Domain layer with business entities
│   │   ├── Common/                # Base classes
│   │   ├── Entities/              # Domain entities
│   │   ├── Enums/                 # Enumerations
│   │   └── Exceptions/            # Domain exceptions
│   │
│   └── ApiTemplate.Infrastructure/# Infrastructure implementations
│       ├── Data/                  # Database context and config
│       ├── Identity/              # Authentication services
│       └── Repositories/          # Repository implementations
│
├── tests/
│   ├── ApiTemplate.UnitTests/     # Unit tests
│   └── ApiTemplate.IntegrationTests/ # Integration tests
│
├── .github/
│   └── workflows/                 # GitHub Actions workflows
│
├── Dockerfile                     # Docker build file
├── docker-compose.yml             # Docker Compose config
└── README.md                      # This documentation
```

## 🔄 Adding a New Entity

To add a new entity to your project:

1. **Domain Layer**: Create the entity class in `Domain/Entities/`
2. **Infrastructure Layer**: Add DbSet and configuration in `ApplicationDbContext`
3. **Application Layer**: Create DTO and service interface/implementation
4. **API Layer**: Create a controller for the entity

Example workflow for adding a "Product" entity:

```csharp
// 1. Domain/Entities/Product.cs
public class Product : AuditableEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    
    private Product() { } // For EF Core
    
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
    
    public void UpdateDetails(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

// 2. Infrastructure/Data/Configurations/ProductConfiguration.cs
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);
    }
}

// 3. Application/Interfaces/IProductService.cs
public interface IProductService
{
    Task<ApiResponse<ProductDto>> GetByIdAsync(Guid id);
    // Other methods...
}

// 4. API/Controllers/ProductsController.cs
[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseApiController
{
    private readonly IProductService _productService;
    
    // Constructor and methods...
}
```

## 🐳 Docker Support

### Building with Docker

```bash
docker build -t your-api-name .
```

### Running with Docker Compose

```bash
docker-compose up -d
```

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/ApiTemplate.UnitTests

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request