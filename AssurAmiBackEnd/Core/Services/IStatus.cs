using AssurAmiBackEnd.Core.Entity;

namespace AssurAmiBackEnd.Core.Services
{
    public interface IStatus
    {
        Task<(IEnumerable<FileStatus> fileStatuses, int TotalCount)> GetAllStatusAsync(int pageNumber, int pageSize);
    }
}
