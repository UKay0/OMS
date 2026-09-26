# OMS.Domain

The innermost layer of the Clean Architecture solution.

**Contains:** entities and enums that describe the business, and nothing else.

**Allowed dependencies:** none. This project must never reference `OMS.Application`,
`OMS.Infrastructure`, `OMS.Web`, or any framework package (EF Core, ASP.NET Core, etc).
That is what makes it the "core" — every other layer depends on it, it depends on nothing.

**When adding a new feature:** put its entity class under `Entities/`. If several
entities share the same audit fields, have them inherit `Common.BaseEntity`.
