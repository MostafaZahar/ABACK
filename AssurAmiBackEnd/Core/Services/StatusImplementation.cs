using AssurAmiBackEnd.Core.Entity;
using AssurAmiBackEnd.Infrastructure.Persistance.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AssurAmiBackEnd.Core.Services
{
    public class StatusImplementation : IStatus
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StatusImplementation(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _context = context;

        }
        public async Task<(IEnumerable<FileStatus> fileStatuses, int TotalCount)> GetAllStatusAsync(int pageNumber, int pageSize)
        {
            var totalStatus = await _context.FileStatuses.CountAsync();
            var clients = await _context.FileStatuses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (clients, totalStatus);
        }
    }
}
