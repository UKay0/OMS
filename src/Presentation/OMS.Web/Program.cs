using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OMS.Infrastructure;
using OMS.Infrastructure.Persistence;
using OMS.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Wires up AppDbContext + every feature service (Employee, Department, Auth, User).
// Adding a new feature never touches this file — see OMS.Infrastructure/DependencyInjection.cs.
builder.Services.AddInfrastructure(builder.Configuration);

// Cookie auth: Login.razor signs in via HttpContext.SignInAsync, Logout via SignOutAsync.
// AddAuthorizationCore (not AddAuthorization) is what makes [Authorize]/AuthorizeView work in Blazor components.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Apply any pending EF Core migrations and ensure the starting accounts and data
// exist, so a fresh deployment works against a brand-new LocalDB with zero manual
// setup.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.SeedAsync(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Minimal endpoint for logout - kept as a plain endpoint (not a Razor page) because
// signing out needs to write a cookie via HttpContext, which is simplest outside the
// Blazor render pipeline.
app.MapPost("/logout", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.Run();
