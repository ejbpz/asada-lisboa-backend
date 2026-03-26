using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Resend;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.FileSystem;
using AsadaLisboaBackend.Middlewares;
using AsadaLisboaBackend.Services.Jwt;
using AsadaLisboaBackend.ErrorHandling;
using AsadaLisboaBackend.Services.News;
using AsadaLisboaBackend.Services.Image;
using AsadaLisboaBackend.Services.Email;
using AsadaLisboaBackend.Services.Users;
using AsadaLisboaBackend.Services.Editor;
using AsadaLisboaBackend.Services.Account;
using AsadaLisboaBackend.Services.Contacts;
using AsadaLisboaBackend.Repositories.News;
using AsadaLisboaBackend.Services.ReCaptcha;
using AsadaLisboaBackend.Repositories.Users;
using AsadaLisboaBackend.Repositories.Images;
using AsadaLisboaBackend.ServiceContracts.Jwt;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Repositories.Contacts;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Editor;
using AsadaLisboaBackend.Services.Configurations;
using AsadaLisboaBackend.ServiceContracts.Account;
using AsadaLisboaBackend.Services.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.RepositoryContracts.Users;
using AsadaLisboaBackend.ServiceContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.ReCaptcha;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.Repositories.Configurations;
using AsadaLisboaBackend.ServiceContracts.FileSystem;
using AsadaLisboaBackend.Repositories.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.Contacts;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddExceptionHandler<InvalidRefreshTokenErrorHandling>();
builder.Services.AddExceptionHandler<InvalidAccessTokenErrorHandling>();
builder.Services.AddExceptionHandler<InvalidCredentialsErrorHandling>();
builder.Services.AddExceptionHandler<SecurityTokenErrorHandling>();
builder.Services.AddExceptionHandler<CreateObjectErrorHandling>();
builder.Services.AddExceptionHandler<RegisterUserErrorHandling>();
builder.Services.AddExceptionHandler<UpdateObjectErrorHandling>();
builder.Services.AddExceptionHandler<ArgumentNullErrorHandling>();
builder.Services.AddExceptionHandler<DbUpdateErrorHandling>();
builder.Services.AddExceptionHandler<ArgumentErrorHandling>();
builder.Services.AddExceptionHandler<NotFoundErrorHandling>();
builder.Services.AddExceptionHandler<IdentityErrorHandling>();
builder.Services.AddExceptionHandler<GlobalErrorHandling>();

builder.Services.AddProblemDetails();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AsadaLisboaDB"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<ReCaptchaOptions>(builder.Configuration.GetSection(nameof(ReCaptchaOptions)));
builder.Services.Configure<RefreshJwtOptions>(builder.Configuration.GetSection(nameof(RefreshJwtOptions)));
builder.Services.Configure<ContactEmailOptions>(builder.Configuration.GetSection(nameof(ContactEmailOptions)));

builder.Services.AddScoped<IContactsAdderRepository, ContactsAdderRepository>();
builder.Services.AddScoped<IContactsGetterRepository, ContactsGetterRepository>();
builder.Services.AddScoped<IContactsUpdaterRepository, ContactsUpdaterRepository>();
builder.Services.AddScoped<IContactsDeleterRepository, ContactsDeleterRepository>();

builder.Services.AddScoped<IAboutUsSectionsAdderRepository, AboutUsSectionsAdderRepository>();
builder.Services.AddScoped<IAboutUsSectionsGetterRepository, AboutUsSectionsGetterRepository>();
builder.Services.AddScoped<IAboutUsSectionsUpdaterRepository, AboutUsSectionsUpdaterRepository>();
builder.Services.AddScoped<IAboutUsSectionsDeleterRepository, AboutUsSectionsDeleterRepository>();

builder.Services.AddScoped<IConfigurationsAdderRepository, ConfigurationsAdderRepository>();
builder.Services.AddScoped<IConfigurationsGetterRepository, ConfigurationsGetterRepository>();
builder.Services.AddScoped<IConfigurationsUpdaterRepository, ConfigurationsUpdaterRepository>();
builder.Services.AddScoped<IConfigurationsDeleterRepository, ConfigurationsDeleterRepository>();

builder.Services.AddScoped<IEditorAdderService, EditorAdderService>();

builder.Services.AddScoped<IFileSystemManager, FileSystemManager>();

builder.Services.AddScoped<IImagesGetterRepository, ImagesGetterRepository>();

builder.Services.AddScoped<INewsAdderRepository, NewsAdderRepository>();
builder.Services.AddScoped<INewsUpdaterRepository, NewsUpdaterRepository>();
builder.Services.AddScoped<INewsDeleterRepository, NewsDeleterRepository>();

builder.Services.AddScoped<INewsAdderService, NewsAdderService>();
builder.Services.AddScoped<INewsUpdaterService, NewsUpdaterService>();
builder.Services.AddScoped<INewsDeleterService, NewsDeleterService>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();

builder.Services.AddScoped<IUsersGetterService, UsersGetterService>();
builder.Services.AddScoped<IUsersUpdaterService, UsersUpdaterService>();
builder.Services.AddScoped<IUsersDeleterService, UsersDeleterService>();
builder.Services.AddScoped<IUsersGetterRepository, UsersGetterRepository>();

builder.Services.AddScoped<IContactsAdderService, ContactsAdderService>();
builder.Services.AddScoped<IContactsGetterService, ContactsGetterService>();
builder.Services.AddScoped<IContactsUpdaterService, ContactsUpdaterService>();
builder.Services.AddScoped<IContactsDeleterService, ContactsDeleterService>();

builder.Services.AddScoped<IConfigurationsAdderService, ConfigurationsAdderService>();
builder.Services.AddScoped<IConfigurationsGetterService, ConfigurationsGetterService>();
builder.Services.AddScoped<IConfigurationsUpdaterService, ConfigurationsUpdaterService>();
builder.Services.AddScoped<IConfigurationsDeleterService, ConfigurationsDeleterService>();

builder.Services.AddScoped<IAboutUsSectionsAdderService, AboutUsSectionsAdderService>();
builder.Services.AddScoped<IAboutUsSectionsGetterService, AboutUsSectionsGetterService>();
builder.Services.AddScoped<IAboutUsSectionsUpdaterService, AboutUsSectionsUpdaterService>();
builder.Services.AddScoped<IAboutUsSectionsDeleterService, AboutUsSectionsDeleterService>();

builder.Services.AddScoped<IImagesGetterService, ImagesGetterService>();

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IReCaptchaService, ReCaptchaService>();
builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IVerificationCodeService, VerificationCodeService>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.SignIn.RequireConfirmedEmail = true;
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

builder.Services.AddContactRateLimiters();

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtOptions:AUDIENCE"], // Who sent the token.
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtOptions:ISSUER"], // Who issues the token.
            ValidateLifetime = true, // Check if it's up-to-date.
            ValidateIssuerSigningKey = true, // Check if it's the signed key.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:KEY"] ?? "")) // Obtain the key to verify the signing key.
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRateLimiter();

app.UseExceptionHandler();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
