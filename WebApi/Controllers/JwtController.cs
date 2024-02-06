using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JwtController : ControllerBase
    {
        private JwtHelpers _jwtHelpers;

        public JwtController(JwtHelpers jwtHelpers)
        {
            _jwtHelpers = jwtHelpers;
        }

        [AllowAnonymous]
        [HttpGet("GenerateToken")]

        public IActionResult GenerateToken(string username)
        {
            var token = _jwtHelpers.GenerateToken(username);
            return Ok(token);
        }
    }
}
