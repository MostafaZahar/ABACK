using AssurAmiBackEnd.Core.Entity.Authentification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssurAmiBackEnd.Core.Services
{
    public interface IUsers
    {
        Task<(IEnumerable<ApplicationUser> applicationUsers, int totalCount)> GetAllUsersAsync();
        Task<(bool Success, string Message)> disabledAccount(string userId);
        Task<(bool Success, string Message)> enableAccount(string userId);
        Task<(bool Success, string Message)> updatePassword(ApplicationUser applicationUser);
    }
}
