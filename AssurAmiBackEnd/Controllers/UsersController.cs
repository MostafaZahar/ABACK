using AssurAmiBackEnd.Core.Entity.Authentification;
using AssurAmiBackEnd.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssurAmiBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _iUsers;

        public UsersController(IUsers users)
        {
            _iUsers = users;
        }

        [HttpGet("get-users")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            var (users, totalCount) = await _iUsers.GetAllUsersAsync();
            return Ok(new { users, totalCount });
        }
        [HttpPatch("activer-compte")]
        public async Task<IActionResult> EnableCompte([FromBody] UserIdRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest(new { Success = false, Message = "The userId field is required." });
            }

            var result = await _iUsers.enableAccount(request.UserId);

            if (result.Success)
            {
                return Ok(new { Success = true, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Success = false, Message = result.Message });
            }
        }

        [HttpPatch("desactiver-compte")]
        public async Task<IActionResult> DisabledAccount([FromBody] UserIdRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest(new { Success = false, Message = "The userId field is required." });
            }

            var result = await _iUsers.disabledAccount(request.UserId);

            if (result.Success)
            {
                return Ok(new { Success = true, Message = result.Message });
            }
            else
            {
                return BadRequest(new { Success = false, Message = result.Message });
            }
        }
        public class UserIdRequest
        {
            public string UserId { get; set; }
        }
}
    }
