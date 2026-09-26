# OMS.Infrastructure

The implementation layer. Everything that talks to the outside world (the database) lives here.

**Contains:** the EF Core `AppDbContext`, entity configurations, migrations, the
`IEmployeeService`/`IDepartmentService`/`IAuthService`/`IUserService` implementations,
password hashing, and startup seed data.

Most services rely on the implicit transaction each `SaveChangesAsync()` call opens on
its own. When a use case spans more than one `SaveChangesAsync()` that must succeed or
fail together, open an explicit one with `db.Database.BeginTransactionAsync()` and
commit/rollback it yourself — see `DepartmentMergeService` for the reference pattern,
including the `ChangeTracker.Clear()` after a rollback (needed because `AppDbContext`
is Scoped per Blazor Server circuit, not per request, so stale tracked entities would
otherwise leak into the next unrelated save on that circuit).

**Allowed dependencies:** `OMS.Application` (and transitively `OMS.Domain`) + EF Core.
Must never be referenced by `OMS.Application` or `OMS.Domain` — only `OMS.Web` wires
this project in, via `AddInfrastructure()` in `DependencyInjection.cs`.

**When adding a new feature:** add `{Feature}Configuration` under `Persistence/Configurations/`,
implement `I{Feature}Service` under `Services/`, add the `DbSet<T>` to `AppDbContext`, and
register the service in `DependencyInjection.cs`. Then run
`dotnet ef migrations add Add{Feature} -p src/Core/OMS.Infrastructure -s src/Presentation/OMS.Web`.
