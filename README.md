# .NET 8 Minimal API Demo

This API demo mimics the functionality of a simple library system.

The solution consists of a minimal API implementation targeting the .NET 8 framework with an instance of MS SQL Server for persistence.

The solution implements the Command and Query Segregation (CQRS) architectural pattern via the Mediator design pattern.

The persistence layer utilizes the Entity Framework ORM (for both command and query operations).

    To fully benefit from the CQRS pattern, a faster ORM such as Dapper would be preferred for the query side of things.

## Project Structure and Contents

1. LibraryDemo.Api
    - Endpoints, service and middleware configuration.
    - Swagger is enabled in the Development environment to test the endpoints and provide documentation.
    - Endpoints are organized in a modular pattern and register their own dependent services via extenstion methods.
    - All services are registered via extension methods defined in helper classes resulting in a slim and clean Program.cs class.
    - Utilizes role-based authentication using Microsoft Identity. Authorization is handled via cookies.
    - Configuration via the `appsettings.json` file and its override files is handled via the IOptions pattern.

1. LibraryDemo.Core
    - DTO classes.
    - Command classes responsible for changing the state of the system.
    - Query classes that return results without any side effects.
    - Validator classes for both the command and query classes.

1. LibraryDemo.Models
    - Domain entities.
    - Identity entities.
    - Configuration classes.
    - Common base classes/interfaces.

1. LibraryDemo.Data
    - Database context classes.
    - A generic repository implementation.
    - A class for seeding data with Bogus.
    - Interceptor for auditing via SaveChanges.
    - Interceptor for soft deleting that overrides default delete behavior.
    - Migrations for each database context.

1. LibraryDemo.Tests
    - Unit tests for commands, validators, and API methods.

## Containerization

The `Dockerfile` for the API and test projects is within the base directory of the solution.

It defines multi-stage builds to optimize image size and keep the file clean.

Three `compose.yml` files are included in the solution to facilitate multi-container builds; 1 base file with 2 override files depending on environment.

## Sample commands:

Build images and run containers in debug configuration.
```console
docker compose -f compose.yml -f compose.debug.yml up -d
```

Stop and remove all containers and associated images for the project.
```console
docker compose down --rmi all
```

Build an image and run the unit tests in a container.
```console
docker compose up tests -d
```