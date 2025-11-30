using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projekt_befit.Data;
using Projekt_befit.Models;

var builder = WebApplication.CreateBuilder(args);

// połączenie do SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();   


var app = builder.Build();
// Tworzenie roli Administrator i konta admina przy starcie aplikacji
using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


	if (!await roleManager.RoleExistsAsync("Administrator"))
	{
		await roleManager.CreateAsync(new IdentityRole("Administrator"));
	}


	string adminEmail = "admin@gmail.com";
	string adminPassword = "Admin123!";

	var adminUser = await userManager.FindByEmailAsync(adminEmail);

	
	if (adminUser == null)
	{
		adminUser = new ApplicationUser
		{
			UserName = adminEmail,
			Email = adminEmail,
			EmailConfirmed = true
		};

		var result = await userManager.CreateAsync(adminUser, adminPassword);

		if (result.Succeeded)
		{
			await userManager.AddToRoleAsync(adminUser, "Administrator");
		}
		else
		{
			throw new Exception("Nie udało się stworzyć konta administratora: "
				+ string.Join(", ", result.Errors.Select(e => e.Description)));
		}
	}
	else
	{
		
		if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
		{
			await userManager.AddToRoleAsync(adminUser, "Administrator");
		}
	}
}

if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
