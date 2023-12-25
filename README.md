# Shop It Books

## Overview

This .NET ASP.NET Web API project showcases the Mediator pattern, Autofac for dependency injection, Amazon S3 for image storage, and Seq for structured logging. Auth0 is used for authentication.

### Key Components

- **Mediator Pattern:** The project embraces the Mediator pattern to simplify communication between components. This pattern promotes loose coupling and enhances maintainability by allowing components to communicate without direct dependencies.

- **Autofac for Dependency Injection:** Autofac is used for dependency injection, enabling the application to manage and resolve dependencies efficiently. This promotes modularity and testability throughout the project.

- **Amazon S3 for Image Storage:** Images are stored in Amazon S3, a scalable and secure object storage service. This ensures reliable and efficient handling of image resources within the application.

- **Seq for Structured Logging:** Seq is employed for structured logging, providing a powerful and searchable log management solution. Structured logs enhance debugging and troubleshooting capabilities, aiding developers in identifying and resolving issues.

- **Auth0 for Authentication:** Auth0 is integrated into the project for secure authentication and authorization. Auth0 is a robust identity management platform that simplifies user authentication and provides features for managing user identities, securing APIs, and implementing authorization policies.
- 
### Configuration

The project's configuration is managed through the `appsettings.json` file. This file includes settings for Serilog (logging), database connection strings, AWS S3 credentials, Auth0 authentication details, and more. Make sure to update these configurations with your specific values before running the application.

```json
{
  "Serilog": {
    // ... Serilog configuration
  },
  "ConnectionStrings": {
    // ... Database connection string
  },
  "AwsConfiguration": {
    // ... AWS S3 credentials
  },
  "Auth0": {
    // ... Auth0 authentication details
  },
  "SwaggerUISecurityMode": "oauth2"
}
```
### Prerequisites

Ensure you have .NET SDK, Autofac, AWS S3 credentials, Seq Logging Server, and an Auth0 account.

## Getting Started

1. Clone the repository:

    ```bash
    git clone https://github.com/aco-zrnic/ShopIt.git
    ```

2. Build and run the project:

    ```bash
    cd your-repo
    dotnet build
    dotnet run
    ```

For more details, refer to the [full README](README.md).

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Dependencies](#dependencies)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Dependencies

- [Autofac](https://autofac.org/)
- [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/)
- [Seq](https://datalust.co/seq)
- [Auth0](https://auth0.com/)

## Usage

Instructions on how to use the project, API endpoints, authentication flows, and examples.

## Contributing

Feel free to contribute by opening issues, providing feedback, or submitting pull requests. Please follow the [contributing guidelines](CONTRIBUTING.md).

## License

This project is licensed under the [MIT License](LICENSE).
