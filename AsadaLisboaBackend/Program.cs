using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AsadaLisboaBackend.Middlewares;
using AsadaLisboaBackend.ServicesExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.ErrorsHandlersRegistration();

builder.Services.AddProblemDetails();

builder.Services.DatabasesRegistration(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.ServicesRegistration();

builder.Services.SendEmailsRegistration();

builder.Services.AddContactRateLimiters();

builder.Services.AddHttpClient();

// Add to service extensions
builder.Services.AddTransient<IImageService, ImageService>();

builder.Services.AddTransient<IDocumentService, DocumentService>();

builder.Services.AddTransient<IDocumentGetterRepository, DocumentGetterRepository>();

builder.Services.AuthenticationsRegistration(builder.Configuration);

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
