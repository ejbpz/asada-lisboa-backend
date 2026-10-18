using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Role;
using AsadaLisboaBackend.ServiceContracts.Roles;

namespace AsadaLisboaBackend.Areas.Admin.Controllers;

/// <summary>
/// Controller for manage roles, only accesible by admin users.
/// </summary>
[ApiController]
[Area("Admin")]
[ApiVersion("1.0")]
[Route("api/[area]/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolesGetterService _rolesGetterService;

    /// <summary>
    /// Constructor for RolesController.
    /// </summary>
    /// <param name="rolesGetterService">Service for retrieving roles.</param>
    public RolesController(IRolesGetterService rolesGetterService)
    {
        _rolesGetterService = rolesGetterService;
    }

    /// <summary>
    /// Retrieves a list of all roles.
    /// </summary>
    /// <returns>A list of RoleResponseDTO objects.</returns>
    [HttpGet("")]
    public async Task<ActionResult<List<RoleResponseDto>>> GetRoles()
    {
        return Ok(await _rolesGetterService.GetRoles());
    }
}