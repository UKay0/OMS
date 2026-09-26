# OMS — .NET 10 Blazor Template

A reference application establishing the architecture, auth model, navigation shell, and
CRUD conventions that future OMS pages/features should follow.

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
seeds two accounts:

| Username | Password    | Role  |
|----------|-------------|-------|
| admin    | Admin@123   | Admin |
| user     | User@123    | User  |

## What this template demonstrates

- **`/welcome`** — the one page anyone can view without signing in.
- **`/login`** — validates against the seeded `Users` table (not a code-level hardcoded bypass).
- **`/employees`** — CRUD with **add/edit via a modal** (`EmployeeFormModal.razor`).
- **`/departments`** — CRUD with **add/edit via a dedicated page** (`DepartmentEdit.razor`).
- **`/admin/users`** — an `[Authorize(Roles = "Admin")]` page, hidden from non-admins in the
  left nav and blocked server-side even via direct URL.
- Top bar: app icon (far left), user avatar + dropdown for Account Settings/Logout (far right).
- Left nav: filtered per signed-in user's role.

## Adding a new feature

Follow the vertical-slice pattern documented in each project's `README.md`:

1. `OMS.Domain/Entities/{Feature}.cs`
2. `OMS.Application/Interfaces/I{Feature}Service.cs` + `Dtos/{Feature}Dto.cs`
3. `OMS.Infrastructure/Persistence/Configurations/{Feature}Configuration.cs` + `Services/{Feature}Service.cs`, add the `DbSet<T>` to `AppDbContext`, register the service in `DependencyInjection.cs`
4. `dotnet ef migrations add Add{Feature} -p src/Core/OMS.Infrastructure -s src/Presentation/OMS.Web`
5. `OMS.Web/Components/Pages/{Feature}/` — a list page plus either a modal (`Employees` pattern) or a dedicated edit page (`Departments` pattern)
6. One line in `NavMenu.razor`

No existing file beyond steps 3's DI line and step 6's nav line needs to change.
