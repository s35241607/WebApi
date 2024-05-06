using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtController : ControllerBase
    {
        private readonly JwtHelpers _jwtHelpers;
        private readonly ILogger<JwtController> _logger;

        public JwtController(JwtHelpers jwtHelpers, ILogger<JwtController> logger)
        {
            _jwtHelpers = jwtHelpers;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("GenerateToken")]

        public IActionResult GenerateToken(string username)
        {
            _logger.LogInformation("user:" + username);
            var token = _jwtHelpers.GenerateToken(username);
            return Ok(token);
        }
    }
}
