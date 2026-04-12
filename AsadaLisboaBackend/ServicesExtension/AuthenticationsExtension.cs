using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to Identity Authentication.
    /// </summary>
    public static class AuthenticationsExtension
    {
        /// <summary>
        /// Identity authentication registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Access to the system configurations.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection AuthenticationsRegistration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        RoleClaimType = ClaimTypes.Role,
                        ValidateAudience = true,
                        ValidAudience = configuration["JwtOptions:AUDIENCE"], // Who sent the token.
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtOptions:ISSUER"], // Who issues the token.
                        ValidateLifetime = true, // Check if it's up-to-date.
                        ValidateIssuerSigningKey = true, // Check if it's the signed key.
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:KEY"] ?? "")) // Obtain the key to verify the signing key.
                    };
    });

            return services;
        }
    }
}
