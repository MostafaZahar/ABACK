using AssurAmiBackEnd.Core.Entity;
using AssurAmiBackEnd.Infrastructure.Persistance.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AssurAmiBackEnd.Core.Services
{
    public class ClientImplimentation : IClient
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClientImplimentation(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task UploadFile(IFormFile file, string userId)
        {
            var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "UploadedFiles");
            var directoryPathSuccessed = Path.Combine(directoryPath, "SuccessedFile");
            var directoryPathFailed = Path.Combine(directoryPath, "FailedFile");
            Directory.CreateDirectory(directoryPathSuccessed); // Create directory if it doesn't exist
            Directory.CreateDirectory(directoryPathFailed); // Create directory if it doesn't exist
            Console.WriteLine("le dossier est créer avec succès");

            // Use the original file name for saving
            var filePath = Path.Combine(directoryPath, file.FileName);
            var successFilePath = Path.Combine(directoryPathSuccessed, file.FileName);
            var failedFilePath = Path.Combine(directoryPathFailed, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            bool isSuccess = false;
            try
            {
                await this.LoadDataClient(filePath, userId); // Passez userId
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Move the file to the appropriate directory based on success or failure
            if (isSuccess)
            {
                if (File.Exists(successFilePath))
                {
                    File.Delete(successFilePath); // Delete if it already exists
                }
                File.Move(filePath, successFilePath); // Move to success directory
            }
            else
            {
                if (File.Exists(failedFilePath))
                {
                    File.Delete(failedFilePath); // Delete if it already exists
                }
                File.Move(filePath, failedFilePath); // Move to failed directory
            }

            // Enregistrez les informations du fichier uploadé dans la base de données
            var uploadedFile = new UploadedFile
            {
                FileName = file.FileName,
                FilePath = isSuccess ? successFilePath : failedFilePath,
                UploadDate = DateTime.UtcNow,
                UserId = userId,
                IsSuccess = isSuccess
            };

            _context.UploadedFiles.Add(uploadedFile);
            await _context.SaveChangesAsync();
        }



        public async Task LoadDataClient(string filepath, string userId)
        {
            if (filepath != null)
            {
                string? connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string storedProcedureName = "LoadCSVFile";
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FilePath", filepath);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.CommandTimeout = 600; // Augmentez le délai d'attente à 600 secondes (10 minutes)

                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("ok");
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<(IEnumerable<Client> Clients, int TotalCount)> GetAllClientsAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm) || c.Prenom.Contains(searchTerm) || c.Matricule.Contains(searchTerm));
            }

            var totalClients = await query.CountAsync();
            var clients = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (clients, totalClients);
        }



        public async Task<bool> ConsumeClientCodeByName(string clientName)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Name == clientName);
            if (client == null || client.IsConsumed == true)
            {
                return false;
            }

            client.IsConsumed = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> ConsumeClientCodeById(long clientId)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<Client> Clients, int TotalCount)> GetAllClientsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
