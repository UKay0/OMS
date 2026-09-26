# OMS

OMS is a Blazor application for managing an organization's employees and departments.

## Stack

- **.NET 10**, **Blazor Web App** (Interactive Server render mode)
- **Clean Architecture**: `OMS.Domain` → `OMS.Application` → `OMS.Infrastructure` → `OMS.Web`
- **Entity Framework Core** + **SQL Server LocalDB**, migrations applied automatically on startup
- **Bootstrap 5** for styling
- Cookie-based auth with a `Users` table (roles: `Admin`, `User`)

Physical layout groups the projects by role:

```
src/
  Core/
    OMS.Domain
    OMS.Application
    OMS.Infrastructure
  Presentation/
    OMS.Web
```

Each project has its own `README.md` explaining that layer's purpose and dependency rules —
start there when adding a new feature.

## Running it

Requires the .NET 10 SDK and SQL Server LocalDB (`sqllocaldb info` to check it's installed).

```bash
dotnet run --project src/Presentation/OMS.Web
```

On first run the app creates the `OmsDb` LocalDB database, applies EF Core migrations, and
seeds an administrator account (`admin` / `Admin@123`) plus a standard user account
(`user` / `User@123`).

## Features

- Public landing page, with everything else requiring sign-in.
- Employee management with add/edit via a modal dialog.
- Department management with add/edit via a dedicated page.
- Department merge: moves every employee out of one department into another and
  removes the original department, as a single database transaction.
- Role-based access: an admin-only User Management page, hidden from and blocked for
  non-admin users.
- Top bar with account menu (Account Settings/Logout); left nav filtered by role.

## Adding a new feature

Follow the vertical-slice pattern documented in each project's `README.md`:

1. `OMS.Domain/Entities/{Feature}.cs`
2. `OMS.Application/Interfaces/I{Feature}Service.cs` + `Dtos/{Feature}Dto.cs`
3. `OMS.Infrastructure/Persistence/Configurations/{Feature}Configuration.cs` + `Services/{Feature}Service.cs`, add the `DbSet<T>` to `AppDbContext`, register the service in `DependencyInjection.cs`
4. `dotnet ef migrations add Add{Feature} -p src/Core/OMS.Infrastructure -s src/Presentation/OMS.Web`
5. `OMS.Web/Components/Pages/{Feature}/` — a list page plus either a modal (`Employees` pattern) or a dedicated edit page (`Departments` pattern)
6. One line in `NavMenu.razor`

No existing file beyond steps 3's DI line and step 6's nav line needs to change.
