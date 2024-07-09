using AssurAmiBackEnd.Core.Services;
using AssurAmiBackEnd.Infrastructure.Persistance.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssurAmiBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClient _clientService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public ClientsController(IWebHostEnvironment webHostEnvironment, IClient clientService, AppDbContext context)
        {
            _clientService = clientService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> StoredUploadCsvFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is empty." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found" });
            }

            try
            {
                await _clientService.UploadFile(file, userId);
                return Ok(new { message = "uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("consume-code")]
        public async Task<IActionResult> ConsumeCode([FromBody] ConsumeCodeRequest request)
        {
            var result = await _clientService.ConsumeClientCodeByName(request.ClientName);

            if (!result)
            {
                return NotFound(new { message = "Client not found or code already consumed" });
            }

            return Ok(new { message = "Client code consumed successfully" });
        }
        public class ConsumeCodeRequest
        {
            public string ClientName { get; set; }
        }

        [HttpGet("get-clients")]
        public async Task<IActionResult> GetClients(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string searchTermName = "",
            [FromQuery] string searchTermMatricule = "")
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(searchTermName))
            {
                query = query.Where(c => c.Name.Contains(searchTermName) || c.Prenom.Contains(searchTermName));
            }

            if (!string.IsNullOrEmpty(searchTermMatricule))
            {
                query = query.Where(c => c.Matricule.Contains(searchTermMatricule));
            }

           

            var totalCount = await query.CountAsync();
            var clients = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new { clients, totalCount });
        }



        [HttpGet("uploaded-files")]
        public async Task<IActionResult> GetUploadedFiles()
        {
            var uploadedFiles = await _context.UploadedFiles
                .Select(uf => new
                {
                    uf.FileName,
                    uf.FilePath,
                    uf.UploadDate,
                    uf.UserId,
                    Status = uf.IsSuccess ? "Success" : "Failed"
                })
                .ToListAsync();

            return Ok(uploadedFiles);
        }

    }
}
