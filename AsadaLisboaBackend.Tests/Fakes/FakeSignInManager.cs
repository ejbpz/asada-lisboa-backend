using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.Tests.Fakes;

public abstract class FakeSignInManager(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor,
    IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
    IOptions<IdentityOptions> optionsAccessor,
    ILogger<SignInManager<ApplicationUser>> logger,
    IAuthenticationSchemeProvider schemes)
    : SignInManager<ApplicationUser>(userManager,
        contextAccessor,
        claimsFactory,
        optionsAccessor,
        logger,
        schemes);