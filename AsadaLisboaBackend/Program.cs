using Serilog;
using AsadaLisboaBackend.Seeders;
using AsadaLisboaBackend.Middlewares;
using AsadaLisboaBackend.ServicesExtension;
using AsadaLisboaBackend.Utils.OptionsPattern;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ControllerRegistration();

builder.Services.AddMemoryCache();

builder.Services.SwaggerRegistration();
builder.Services.VersioningRegistration();

builder.Services.OptionsPatternRegistration(builder.Configuration);

builder.Services.ErrorsHandlersRegistration();

builder.Services.AddProblemDetails();

builder.Services.DatabasesRegistration(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.ServicesRegistration();

builder.Services.SendEmailsRegistration();

builder.Services.AddContactRateLimiters();

builder.Services.AddHttpClient();

builder.Services.ReceiptsServicesRegistration(builder.Configuration);

builder.Services.AuthenticationsRegistration(builder.Configuration);
builder.Services.AuthorizationsRegistration();

builder.Services.SerilogRegistration(builder.Host);

builder.Services.CorsRegistration();
builder.Services.ForwardedHeadersRegistration();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseForwardedHeaders();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.InjectStylesheet("/css/swagger-styles.css");
    });
}

await ApplicationDbSeeder.SeedAsync(app.Services);

var config = app.Services
    .GetRequiredService<IConfiguration>()
    .GetSection("DefaultUserOptions")
    .Get<DefaultUserOptions>();

if (config is not null && config.RUN)
{
    await app.Services.SeedAdminUserAsync();
}

app.UseSerilogRequestLogging();

app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}
