using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.API.Controllers;

/// <summary>
/// Base controller for all API controllers
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}