# RealEstate API

This repository contains the backend of the **RealEstate API**, a comprehensive solution for a home-selling project. Developed with **ASP.NET Core Web API**, it adheres to modern software design principles, including **N-Tier Architecture** and **Entity Framework Core** (Code-First). Below are the key features and technologies implemented.

## Technologies & Tools
- **.NET 8**: Latest framework version for high performance and scalability.
- **ASP.NET Core Web API**: RESTful API framework.
- **Entity Framework Core**: Code-first approach for database modeling.
- **Docker**: Used for containerization and deployment through `docker-compose.yml`.
- **MiniProfiler**: Added for request profiling.
- **Sentry**: Handles unhandled exceptions.
- **Action Filters**: For logging and custom request/response processing.

## Features
- **Authentication & Authorization**: Implemented with custom middleware and token blacklisting.
- **Custom Validation**: Token validation and other request-level checks.
- **Database Logging**: Comprehensive logging through database action filters.
- **Global Exception Handling**: Robust error management.
- **Security Headers**: Ensured response security with headers.
- **Audit Properties**: Tracked changes by overriding `SaveChangesAsync`.
- **Automapper Configuration**: For DTO to entity mapping.
- **Generic CRUD Operations**: Implemented for multiple entities.
- **Pagination**: Custom generic pagination functionality.
- **Localization**: Multi-language support.
- **Mail Sender**: Integrated mail-sending service.
- **Custom Automapping & Validation**: Between entities and DTOs.
- **Encoding/Decoding**: Added for secure data handling.

## Architecture Layers
- **API Layer**: Handles HTTP requests and routing.
- **BLL (Business Logic Layer)**: Contains business rules and logic.
- **Core Layer**: Provides essential utilities and cross-cutting concerns.
- **DAL (Data Access Layer)**: Manages database access using EF Core.
- **DTO (Data Transfer Objects Layer)**: Facilitates data exchange between layers.
- **Entities Layer**: Represents the database models/entities.
