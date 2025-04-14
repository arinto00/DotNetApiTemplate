# .NET API Template

A clean architecture API template for .NET, designed to provide a solid foundation for building microservices with best practices.

## Features

- **Clean Architecture** - Domain, Application, Infrastructure, and API layers
- **CQRS Pattern** - Using services and repositories
- **JWT Authentication** - Secure your API with JWT tokens
- **Swagger Documentation** - API documentation out of the box
- **Health Checks** - Monitor the health of your API and its dependencies
- **Repository Pattern** - Abstract data access with EF Core
- **Sample Entity** - A complete implementation of a sample entity
- **Exception Handling** - Global exception handling middleware
- **Dependency Injection** - Well-organized DI setup
- **Entity Auditing** - Track creation and modification of entities

## Getting Started

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download) or later
- A database (SQL Server, PostgreSQL, SQLite, etc.)

### Database Setup

This template is configured to use SQL Server by default. For macOS/Linux users, you can:

1. Use Docker to run SQL Server:
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
   ```

2. Or use PostgreSQL by changing the connection string and provider in `Infrastructure/DependencyInjection.cs`:
   ```csharp
   services.AddDbContext<ApplicationDbContext>(options =>
       options.UseNpgsql(
           configuration.GetConnectionString("DefaultConnection"),
           b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
   ```

3. Or use SQLite for development:
   ```csharp
   services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlite(
           configuration.GetConnectionString("DefaultConnection"),
           b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
   ```

Update the connection string in `appsettings.json` accordingly.

### Running the API

1. Clone this repository
2. Update the connection string in `appsettings.json`
3. Run the following commands:

```bash
cd src/ApiTemplate.API
dotnet run
```

The API will be available at https://localhost:7001 (HTTPS) and http://localhost:5163 (HTTP), with Swagger documentation accessible at the root URL.

## Project Structure

```
ApiTemplate/
├── src/
│   ├── ApiTemplate.API/           # API layer with controllers and configuration
│   ├── ApiTemplate.Application/   # Application layer with services and DTOs
│   ├── ApiTemplate.Domain/        # Domain layer with entities
│   └── ApiTemplate.Infrastructure/# Infrastructure layer with repositories
└── tests/
    ├── ApiTemplate.UnitTests/     # Unit tests
    └── ApiTemplate.IntegrationTests/ # Integration tests
```

## Architecture

This template follows the principles of Clean Architecture:

1. **Domain Layer**: Contains enterprise logic and entities
2. **Application Layer**: Contains business logic and interfaces
3. **Infrastructure Layer**: Contains implementation of interfaces
4. **API Layer**: Contains controllers and configuration

## Customizing the Template

To use this template for your own project:

1. Rename the solution and projects
2. Replace `Sample` with your own entity
3. Update the connection string
4. Update JWT settings
5. Update Swagger documentation

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.