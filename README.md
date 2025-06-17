# Dynamic Authentication Providers Samples

This repository contains sample implementations of Dynamic Authentication Providers (DAPs) using different configuration approaches. These samples demonstrate how to integrate the Rock Solid Knowledge Dynamic Authentication Providers component with ASP.NET Core and IdentityServer.

## Overview

The Dynamic Authentication Providers component is available from [identityserver.com](https://www.identityserver.com/products/dynamic-authentication-providers) and allows ASP.NET Core applications to load authentication configuration from a database during runtime. New providers can be added during runtime without restarting the application.

## Sample Projects

The samples demonstrate a progression from basic to advanced usage:

- **Core** - Basic implementation showing in-memory configuration of dynamic providers
- **EntityFramework** - Advanced implementation demonstrating database persistence using Entity Framework
- **CustomProvider** - Example of implementing and configuring custom authentication provider options
- **IdentityServer** - Integration sample showing Dynamic Authentication Providers with IdentityServer

## Supported Providers

The Dynamic Authentication Providers component supports multiple authentication protocols:
- OpenID Connect (OIDC) Providers
- WS-Federation Providers 
- SAML 2.0 Providers (requires [RSK SAML Component](https://www.identityserver.com/products/saml2p))

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or JetBrains Rider
- Valid license key for the Dynamic Authentication Providers component

## Quick Start

1. Clone this repository
2. Obtain a license key (see below)
3. Configure the license key in the projects
4. Run the desired sample projects

## Documentation

For detailed documentation on implementing and using Dynamic Authentication Providers:
- [Developer Documentation](https://docs.identityserver.com/dynamic-authentication-providers/)
- [Introduction Article](https://www.identityserver.com/articles/dynamic-authentication-providers)

Key features:
- Add/configure/remove authentication providers at runtime without code changes or restarts
- Designed for ASP.NET Core authentication system
- Compatible with both Duende IdentityServer and IdentityServer4
- Essential for building multi-tenancy federated identity solutions

## Obtaining a License

The Dynamic Authentication Providers component requires a commercial license. You can obtain a license key in one of two ways:
- Get a free 30-day demo key from our [products page](https://www.identityserver.com/products/dynamic-authentication-providers)
- Purchase an Enterprise license by contacting sales@identityserver.com

## Support

For support questions:
- Review our [documentation](https://www.identityserver.com/documentation/)
- Contact our support team at support@identityserver.com
- Enterprise customers can access our dedicated helpdesk

## License

This sample code is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.

The Dynamic Authentication Providers component requires a commercial license from Rock Solid Knowledge.
