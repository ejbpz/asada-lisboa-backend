using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Resend;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Services.Email;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Services.Account;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.ServiceContracts.Account;
using AsadaLisboaBackend.Repositories.Users;
using AsadaLisboaBackend.RepositoryContracts.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AsadaLisboaDB"));
});

builder.Services.AddTransient<IUsersGetterRepository, UsersGetterRepository>();
builder.Services.AddTransient<IUsersUpdaterRepository, UsersUpdaterRepository>();
builder.Services.AddTransient<IUsersDeleterRepository, UsersDeleterRepository>();

builder.Services.AddTransient<IUsersGetterService, UsersGetterService>();
builder.Services.AddTransient<IUsersUpdaterService, UsersUpdaterService>();
builder.Services.AddTransient<IUsersDeleterService, UsersDeleterService>();

builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<IResetPasswordService, ResetPasswordService>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
    .AddDefaultTokenProviders();

builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = Environment.GetEnvironmentVariable(Constants.RESEND_API_TOKEN.Trim())!;
});
builder.Services.AddTransient<IResend, ResendClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
