using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AsadaLisboaBackend.Conventions;
using AsadaLisboaBackend.Middlewares;
using AsadaLisboaBackend.ServicesExtension;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new AuthorizationConvention());
}).AddXmlSerializerFormatters();

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

builder.Services.AuthenticationsRegistration(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constants.ROLE_ADMINISTRADOR, p => p.RequireRole(Constants.ROLE_ADMINISTRADOR));
    options.AddPolicy(Constants.ROLE_EDITOR, p => p.RequireRole(Constants.ROLE_EDITOR, Constants.ROLE_ADMINISTRADOR));
    options.AddPolicy(Constants.ROLE_LECTOR, p => p.RequireRole(Constants.ROLE_LECTOR, Constants.ROLE_EDITOR, Constants.ROLE_ADMINISTRADOR));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRateLimiter();

app.UseExceptionHandler();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.InjectStylesheet("/css/swagger-styles.css");
    });
}

var config = app.Services
    .GetRequiredService<IConfiguration>()
    .GetSection("DefaultUserOptions")
    .Get<DefaultUserOptions>();

if (config is not null && config.RUN)
    await app.Services.SeedAdminUserAsync();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
