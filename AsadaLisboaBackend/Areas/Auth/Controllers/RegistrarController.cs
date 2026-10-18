using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Areas.Auth.Controllers;

/// <summary>
/// Controller for manage registration.
/// </summary>
[ApiController]
[Area("Auth")]
[ApiVersion("1.0")]
[Route("api/[area]/[controller]")]
public class RegistrarController : ControllerBase
{
    private readonly IRegisterUserService _userService;
    private readonly IVerificationCodeService _verificationCodeService;

    /// <summary>
    /// Constructor for RegistrarController.
    /// </summary>
    /// <param name="userService">Service for user management.</param>
    /// <param name="verificationCodeService">Service for creating verification code.</param>
    public RegistrarController(IRegisterUserService userService, IVerificationCodeService verificationCodeService)
    {
        _userService = userService;
        _verificationCodeService = verificationCodeService;
    }

    /// <summary>
    /// Creates a new user based on the provided RegisterRequestDTO object.
    /// </summary>
    /// <param name="registerRequestDto">An object containing the details of the user to be created. Cannot be null.</param>
    /// <returns>Create confirmation.</returns>
    [HttpPost("")]
    [Authorize(Roles = Constants.ROLE_ADMINISTRADOR)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto registerRequestDto)
    {
        await _userService.RegisterUser(registerRequestDto);
        return Created();
    }

    /// <summary>
    /// Confirms the user email sent by email.
    /// </summary>
    /// <param name="verificationCodeRequestDto">An object containing the details of the verification code to be confirmed</param>
    /// <returns>No content.</returns>
    [HttpPost("confirmar-correo")]
    public async Task<IActionResult> VerifyEmail([FromQuery] VerificationCodeRequestDto verificationCodeRequestDto)
    {
        await _verificationCodeService.ConfirmEmailAsync(verificationCodeRequestDto.Email, verificationCodeRequestDto.Token);
        return NoContent();
    }
}