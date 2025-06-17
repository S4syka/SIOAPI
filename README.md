# SIO API

SIO is a lightweight backend API for a small learning management system (LMS). It provides endpoints for creating tests, organising them into homework assignments and tracking student progress. Only administration admins are expected to author new tests using the test editor, while users interact with the tests and homeworks assigned to them.

## Overview

At its core the LMS stores tests that can be grouped together using tags and categories. Users create groups of other users and invite members to those groups. After members accept their invitation, the group owner can create homework that references one or more tests. Each time a student writes a test the answers are recorded as a *write* entry. If that test belongs to a homework the student can hand in the write so the owner can review it.

The key entities are:

- **User** – represents both admins and users.
- **Group** – collection of users with an owner. Members join via an invitation.
- **Homework** – list of tests assigned to a group with a due date.
- **Test** – question set that can be tagged with multiple tags.
- **Tag** and **Category** – tags link tests to a category for easy filtering.
- **Write** – a specific attempt of a user writing a test; stores answers and results.
- **Handin** – ties a write to a homework when a student submits their attempt.

A typical workflow is:

1. An admin creates one or more tests using the editor.
2. User creates a group and invites other users by their user name.
3. Once the users accept the invitation the owner of group creates homework and selects which tests are required.
4. Users write each test individually. If the test is part of a homework they can hand in the attempt for review.

## Project layout

- `Contracts` – repository interfaces used by the services layer.
- `DataTransferObjects` – request and response objects exposed by endpoints.
- `Model` – Entity Framework models and `OaDbContext` containing the database schema.
- `Repository` and `Repository.S3` – abstractions and implementations for data access and S3-based file storage.
- `Services` – domain logic for tests and images.
- `MyWebApp` – the ASP.NET FastEndpoints API project.
- `MyWebApp.Tests` – xUnit test project covering the API.

## Running the API

The application targets .NET 9 and expects a PostgreSQL database available on `localhost` with default credentials as configured in `OaDbContext`. To start the API locally:

```bash
# ensure the .NET 9 SDK is installed
cd MyWebApp
 dotnet run
```

Swagger UI will be available at the root URL for exploring the endpoints.

## Testing

Execute all unit tests with:

```bash
dotnet test
```