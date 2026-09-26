# OMS.Application

The use-case layer. Defines *what* the app can do, not *how*.

**Contains:** service interfaces (contracts), DTOs, and form models used by the UI.

**Allowed dependencies:** `OMS.Domain` only. Must never reference `OMS.Infrastructure`
or `OMS.Web` — those depend on this layer, not the other way around. This is what lets
Infrastructure be swapped (different DB, different ORM) without touching Application or Web.

**When adding a new feature:** add `I{Feature}Service` under `Interfaces/` and its
DTO/FormModel under `Dtos/`. `OMS.Infrastructure` will implement the interface;
`OMS.Web` will consume it via DI — this project never contains the implementation.
