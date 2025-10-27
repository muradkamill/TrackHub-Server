using Application.Auth;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand.RegisterRequest request,CancellationToken cancellationToken)
        {
            var person = await sender.Send(request, cancellationToken);
            if (person.IsFailed)
            {
                return BadRequest(person.Errors.Select(x => x.Message));
            }

            return Created();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand.LoginRequest request,CancellationToken cancellationToken)
        {
            var person = await sender.Send(request, cancellationToken);
            if (person.IsFailed)
            {
                return BadRequest(person.Errors.Select(x => x.Message));
            }
        
            return Ok(person.Value);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshCommand.RefreshRequest request,CancellationToken cancellationToken)
        {
            var person = await sender.Send(request, cancellationToken);
            if (person.IsFailed)
            {
                return BadRequest(person.Errors.Select(x => x.Message));
            }
        
            return Ok(person.Value);
        }

        [Authorize]
        [HttpGet("authorize")]
        public IActionResult Authorize()
        {
            return Ok("You are Authorized!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult adminOnly()
        {
            return Ok("You are Admin!");

        }
        [Authorize(Roles = "Courier")]
        [HttpGet("courier-only")]
        public IActionResult CourierOnly()
        {
            return Ok("You are Courier!");

        } 
    }
}
