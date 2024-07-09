using AssurAmiBackEnd.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;


namespace AssurAmiBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatus _status;
        public StatusController(IStatus statusService)
        {
            _status = statusService;
        }



        [Microsoft.AspNetCore.Mvc.HttpGet("get-status")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetStatus([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var (clients, totalCount) = await _status.GetAllStatusAsync(pageNumber, pageSize);
            return Ok(new { clients, totalCount });
        }
    }
}
