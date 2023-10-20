using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WiseProject.Business.Abstract;
using WiseProject.Business.Concrete;
using WiseProject.Data;
using WiseProject.Data.DAL.Abstract;
using WiseProject.Data.DAL.Concrete;
using WiseProject.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<IEnrollmentDal, EnrollmentDal>();
builder.Services.AddSingleton<IEnrollmentService, EnrollmentManager>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserManager>();

builder.Services.AddScoped<IAssignmentDal, AssignmentDal>();
builder.Services.AddScoped<IAssignmentService, AssignmentManager>();

builder.Services.AddScoped<ICourseDal, CourseDal>();
builder.Services.AddScoped<ICourseService, CourseManager>();
#region [ Cookie  ]

CookieBuilder cookieBuilder = new CookieBuilder
{
    Name = "LMS",
    HttpOnly = false,
    SameSite = SameSiteMode.Lax,
    SecurePolicy = CookieSecurePolicy.SameAsRequest
};

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/Identity/Account/Login";
    opts.LogoutPath = "/Identity/Account/Logout";
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opts.Cookie = cookieBuilder;
    opts.SlidingExpiration = true;
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(30);
});

#endregion

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider;
    try
    {

        UserRoleInit.InitAsync(dbContext).Wait();
    }
    catch (Exception ex)
    {
        var logger = dbContext.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while attempting to seed the database");
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
