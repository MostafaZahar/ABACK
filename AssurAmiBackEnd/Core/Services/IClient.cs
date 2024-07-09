using AssurAmiBackEnd.Core.Entity;

namespace AssurAmiBackEnd.Core.Services
{
    public interface IClient
    {
        Task LoadDataClient(string filepath, string userId);
        Task UploadFile(IFormFile file, string userId);
        Task<(IEnumerable<Client> Clients, int TotalCount)> GetAllClientsAsync(int pageNumber, int pageSize);
        Task<bool> ConsumeClientCodeById(long clientId);
        Task<bool> ConsumeClientCodeByName(string clientName);
    }

}
