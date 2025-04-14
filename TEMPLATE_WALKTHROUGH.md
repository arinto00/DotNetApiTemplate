# .NET API Template - Detailed Walkthrough

This document provides an in-depth exploration of each component in the template, explaining what each file does, how it works, and how to customize it for your needs.

## Table of Contents

- [Domain Layer](#domain-layer)
- [Application Layer](#application-layer)
- [Infrastructure Layer](#infrastructure-layer)
- [API Layer](#api-layer)
- [Testing](#testing)
- [Configuration](#configuration)
- [Authentication](#authentication)
- [Customization Guide](#customization-guide)

---

## Domain Layer

The Domain layer is the core of your application, containing business entities and rules with no dependencies on other layers.

### Key Components

#### BaseEntity.cs

**Location**: `src/ApiTemplate.Domain/Common/BaseEntity.cs`

**Purpose**: Base class for all domain entities, providing common properties like Id.

**How to use**: Inherit from this class when creating new entities to ensure they have a GUID Id.

```csharp
public class YourEntity : BaseEntity
{
    // Your entity properties and methods
}
```

#### AuditableEntity.cs

**Location**: `src/ApiTemplate.Domain/Common/AuditableEntity.cs`

**Purpose**: Extends BaseEntity with audit properties (CreatedAt, CreatedBy, etc.).

**How to use**: Inherit from this class when you need to track entity creation and modification.

```csharp
public class YourAuditedEntity : AuditableEntity
{
    // Your entity will automatically track created/modified info
}
```

#### Sample.cs

**Location**: `src/ApiTemplate.Domain/Entities/Sample.cs`

**Purpose**: Example entity showing proper domain entity design with encapsulation.

**Key features**:
- Private setters to enforce encapsulation
- Private constructor for EF Core
- Public methods for state changes
- Business rules enforced in the entity itself

**How to customize**: Use this as a template for your own entities, following the same patterns.

#### DomainException.cs

**Location**: `src/ApiTemplate.Domain/Exceptions/DomainException.cs`

**Purpose**: Custom exceptions for domain-specific errors.

**How to use**: Create specific exception types for your domain rules.

```csharp
throw new EntityNotFoundException("Product", id);
```

---

## Application Layer

The Application layer contains business logic and orchestrates domain objects to perform tasks.

### Key Components

#### IRepository.cs

**Location**: `src/ApiTemplate.Application/Common/Interfaces/IRepository.cs`

**Purpose**: Generic repository interface defining standard operations for entities.

**How to use**: Use this interface to access entities in services without directly depending on database implementation.

```csharp
public class YourService
{
    private readonly IRepository<YourEntity> _repository;
    
    // Use repository methods like GetByIdAsync, AddAsync, etc.
}
```

#### IApplicationDbContext.cs

**Location**: `src/ApiTemplate.Application/Common/Interfaces/IApplicationDbContext.cs`

**Purpose**: Defines the database context interface for dependency inversion.

**How to customize**: Add DbSet properties for your new entities.

```csharp
public interface IApplicationDbContext
{
    DbSet<YourEntity> YourEntities { get; }
    // Other entity sets...
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
```

#### ApiResponse.cs

**Location**: `src/ApiTemplate.Application/Common/Models/ApiResponse.cs`

**Purpose**: Standardized response model for all API operations.

**How to use**: Wrap all service responses with this class for consistent API responses.

```csharp
return ApiResponse<YourDto>.Success(data);
// or
return ApiResponse<YourDto>.Failure("Error message");
```

#### ISampleService.cs and SampleService.cs

**Location**: 
- `src/ApiTemplate.Application/Interfaces/ISampleService.cs`
- `src/ApiTemplate.Application/Services/SampleService.cs`

**Purpose**: Example service interface and implementation showing proper separation of concerns.

**Key features**:
- DTOs defined in the interface for clear contracts
- Clean separation between API controllers and data access
- Business logic encapsulation
- Error handling and logging

**How to customize**: Create similar service interfaces and implementations for your entities.

#### MappingProfile.cs

**Location**: `src/ApiTemplate.Application/Mapping/MappingProfile.cs`

**Purpose**: AutoMapper configuration for object-to-object mapping.

**How to customize**: Add mapping configurations for your entities and DTOs.

```csharp
CreateMap<YourEntity, YourEntityDto>()
    .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.Property));
```

#### DependencyInjection.cs

**Location**: `src/ApiTemplate.Application/DependencyInjection.cs`

**Purpose**: Registers all application layer services with the DI container.

**How to customize**: Register your new services here.

```csharp
services.AddScoped<IYourService, YourService>();
```

---

## Infrastructure Layer

The Infrastructure layer implements interfaces defined in the Application layer, handling external concerns like databases and authentication.

### Key Components

#### ApplicationDbContext.cs

**Location**: `src/ApiTemplate.Infrastructure/Data/ApplicationDbContext.cs`

**Purpose**: Entity Framework Core database context implementation.

**Key features**:
- Implements IApplicationDbContext
- Handles auditing when entities are saved
- Applies entity configurations

**How to customize**: 
- Add DbSet properties for your entities
- Override SaveChangesAsync for custom behavior

#### ApplicationDbContextInitializer.cs

**Location**: `src/ApiTemplate.Infrastructure/Data/ApplicationDbContextInitializer.cs`

**Purpose**: Handles database initialization and seeding.

**How to customize**: Add seed data for your entities in the TrySeedAsync method.

#### SampleConfiguration.cs

**Location**: `src/ApiTemplate.Infrastructure/Data/Configurations/SampleConfiguration.cs`

**Purpose**: Entity Framework Core configuration for the Sample entity.

**How to customize**: Create similar configuration classes for your entities.

```csharp
public class YourEntityConfiguration : IEntityTypeConfiguration<YourEntity>
{
    public void Configure(EntityTypeBuilder<YourEntity> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        // More configuration...
    }
}
```

#### GenericRepository.cs

**Location**: `src/ApiTemplate.Infrastructure/Repositories/GenericRepository.cs`

**Purpose**: Implementation of the IRepository interface using Entity Framework Core.

**How to use**: This is used automatically when IRepository<T> is injected.

#### CurrentUserService.cs

**Location**: `src/ApiTemplate.Infrastructure/Identity/CurrentUserService.cs`

**Purpose**: Gets the current user from the HTTP context for auditing and permissions.

**How it works**: Extracts user identity from JWT claims.

#### JwtTokenGenerator.cs

**Location**: `src/ApiTemplate.Infrastructure/Identity/JwtTokenGenerator.cs`

**Purpose**: Generates JWT tokens for authentication.

**How to customize**: Modify to add additional claims or change token configuration.

#### DependencyInjection.cs

**Location**: `src/ApiTemplate.Infrastructure/DependencyInjection.cs`

**Purpose**: Registers all infrastructure services with the DI container.

**How to customize**: Add registration for any new infrastructure services.

---

## API Layer

The API layer handles HTTP requests, routing, and presents data to clients.

### Key Components

#### BaseApiController.cs

**Location**: `src/ApiTemplate.API/Controllers/BaseApiController.cs`

**Purpose**: Base controller all other controllers inherit from.

**How to use**: Inherit from this when creating new controllers.

```csharp
public class YourController : BaseApiController
{
    // Your controller methods
}
```

#### SamplesController.cs

**Location**: `src/ApiTemplate.API/Controllers/SamplesController.cs`

**Purpose**: Example controller showing proper API design.

**Key features**:
- Clean routing
- Proper status codes
- Dependency injection
- Documentation with XML comments
- Response type specification

**How to customize**: Create similar controllers for your entities.

#### ExceptionHandlingMiddleware.cs

**Location**: `src/ApiTemplate.API/Middleware/ExceptionHandlingMiddleware.cs`

**Purpose**: Global exception handling for consistent error responses.

**How it works**: Catches exceptions and formats them into standard API responses.

#### AuthenticationExtensions.cs

**Location**: `src/ApiTemplate.API/Extensions/AuthenticationExtensions.cs`

**Purpose**: Configures JWT authentication.

**How to customize**: Modify JWT settings or authentication logic.

#### SwaggerExtensions.cs

**Location**: `src/ApiTemplate.API/Extensions/SwaggerExtensions.cs`

**Purpose**: Configures Swagger/OpenAPI documentation.

**How to customize**: Add additional customization for API documentation.

#### SelfHealthCheck.cs

**Location**: `src/ApiTemplate.API/Extensions/SelfHealthCheck.cs`

**Purpose**: Health check for monitoring API status.

**How to customize**: Add more health checks for external dependencies.

#### Program.cs

**Location**: `src/ApiTemplate.API/Program.cs`

**Purpose**: Application entry point and configuration.

**Key sections**:
- Service registration
- Middleware configuration
- Health checks
- CORS setup
- Database initialization

**How to customize**: Add additional services or middleware.

#### appsettings.json

**Location**: `src/ApiTemplate.API/appsettings.json`

**Purpose**: Application configuration.

**Key sections**:
- Connection strings
- JWT settings
- Logging configuration
- CORS origins

**How to customize**: Add your own configuration sections.

---

## Testing

### Key Components

#### SampleServiceTests.cs

**Location**: `tests/ApiTemplate.UnitTests/Application/Services/SampleServiceTests.cs`

**Purpose**: Example unit tests for the SampleService.

**Key features**:
- Proper mocking with Moq
- Focused test methods
- Arrange/Act/Assert pattern
- Verification of expected behaviors

**How to customize**: Create similar tests for your services.

---

## Configuration

### Dockerfile

**Location**: `Dockerfile`

**Purpose**: Multi-stage Docker build definition.

**How to customize**: Modify base images or build steps if needed.

### docker-compose.yml

**Location**: `docker-compose.yml`

**Purpose**: Docker Compose configuration for local development.

**How to customize**: Add additional services or environment variables.

### .github/workflows/build.yml

**Location**: `.github/workflows/build.yml`

**Purpose**: GitHub Actions CI/CD workflow.

**How to customize**: Add deployment steps or additional checks.

---

## Authentication

The template uses JWT (JSON Web Tokens) for authentication.

### How Authentication Works

1. **Configuration**: JWT settings in appsettings.json
2. **Token Generation**: JwtTokenGenerator creates tokens
3. **Token Validation**: AuthenticationExtensions configures validation
4. **User Identification**: CurrentUserService extracts user info

### Customizing Authentication

To add roles or custom claims:
1. Modify JwtTokenGenerator.cs to include additional claims
2. Add role-based authorization attributes to controllers or actions

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    // Admin-only endpoints
}
```

---

## Customization Guide

### Adding a New Entity

1. **Create the entity in Domain layer**:
   ```csharp
   // src/ApiTemplate.Domain/Entities/Product.cs
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
   ```

2. **Add DbSet to IApplicationDbContext**:
   ```csharp
   // src/ApiTemplate.Application/Common/Interfaces/IApplicationDbContext.cs
   public interface IApplicationDbContext
   {
       DbSet<Product> Products { get; }
       // Other DbSets...
       
       Task<int> SaveChangesAsync(CancellationToken cancellationToken);
   }
   ```

3. **Add entity configuration**:
   ```csharp
   // src/ApiTemplate.Infrastructure/Data/Configurations/ProductConfiguration.cs
   public class ProductConfiguration : IEntityTypeConfiguration<Product>
   {
       public void Configure(EntityTypeBuilder<Product> builder)
       {
           builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
           builder.Property(p => p.Price).HasPrecision(18, 2);
       }
   }
   ```

4. **Add DbSet to ApplicationDbContext**:
   ```csharp
   // src/ApiTemplate.Infrastructure/Data/ApplicationDbContext.cs
   public DbSet<Product> Products => Set<Product>();
   ```

5. **Create DTOs and service interface**:
   ```csharp
   // src/ApiTemplate.Application/Interfaces/IProductService.cs
   public record ProductDto(Guid Id, string Name, decimal Price, DateTime CreatedAt);
   public record CreateProductDto(string Name, decimal Price);
   public record UpdateProductDto(string Name, decimal Price);
   
   public interface IProductService
   {
       Task<ApiResponse<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
       Task<ApiResponse<IEnumerable<ProductDto>>> GetAllAsync(CancellationToken cancellationToken = default);
       Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto createDto, CancellationToken cancellationToken = default);
       Task<ApiResponse<ProductDto>> UpdateAsync(Guid id, UpdateProductDto updateDto, CancellationToken cancellationToken = default);
       Task<ApiResponse<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
   }
   ```

6. **Implement the service**:
   ```csharp
   // src/ApiTemplate.Application/Services/ProductService.cs
   public class ProductService : IProductService
   {
       private readonly IRepository<Product> _repository;
       private readonly IMapper _mapper;
       private readonly ILogger<ProductService> _logger;
       
       public ProductService(IRepository<Product> repository, IMapper mapper, ILogger<ProductService> logger)
       {
           _repository = repository;
           _mapper = mapper;
           _logger = logger;
       }
       
       // Implement interface methods...
   }
   ```

7. **Add AutoMapper configuration**:
   ```csharp
   // src/ApiTemplate.Application/Mapping/MappingProfile.cs
   CreateMap<Product, ProductDto>()
       .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
       .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
       .ForCtorParam("Price", opt => opt.MapFrom(src => src.Price))
       .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt));
   ```

8. **Register the service**:
   ```csharp
   // src/ApiTemplate.Application/DependencyInjection.cs
   services.AddScoped<IProductService, ProductService>();
   ```

9. **Create the controller**:
   ```csharp
   // src/ApiTemplate.API/Controllers/ProductsController.cs
   [ApiController]
   [Route("api/[controller]")]
   public class ProductsController : BaseApiController
   {
       private readonly IProductService _productService;
       
       public ProductsController(IProductService productService)
       {
           _productService = productService;
       }
       
       // Implement endpoints...
   }
   ```

### Adding Validation

For input validation, add FluentValidation:

1. **Create validator class**:
   ```csharp
   // src/ApiTemplate.Application/Features/Products/Validators/CreateProductValidator.cs
   public class CreateProductValidator : AbstractValidator<CreateProductDto>
   {
       public CreateProductValidator()
       {
           RuleFor(p => p.Name)
               .NotEmpty().WithMessage("Name is required")
               .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
               
           RuleFor(p => p.Price)
               .GreaterThan(0).WithMessage("Price must be greater than zero");
       }
   }
   ```

2. **Register validators**:
   ```csharp
   // src/ApiTemplate.Application/DependencyInjection.cs
   services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
   ```

3. **Use validation in services or controllers**.

### Adding Background Services

1. **Create a background service**:
   ```csharp
   // src/ApiTemplate.Infrastructure/BackgroundServices/YourBackgroundService.cs
   public class YourBackgroundService : BackgroundService
   {
       protected override async Task ExecuteAsync(CancellationToken stoppingToken)
       {
           while (!stoppingToken.IsCancellationRequested)
           {
               // Do work...
               await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
           }
       }
   }
   ```

2. **Register the service**:
   ```csharp
   // src/ApiTemplate.Infrastructure/DependencyInjection.cs
   services.AddHostedService<YourBackgroundService>();
   ```

---

This document should be treated as a living guide. As you customize and extend the template, update this document to reflect your changes and best practices.