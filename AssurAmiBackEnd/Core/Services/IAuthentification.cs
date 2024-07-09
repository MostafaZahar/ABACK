using AssurAmiBackEnd.Core.Entity;
using AssurAmiBackEnd.Core.Entity.Authentification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssurAmiBackEnd.Core.Services
{
    public  interface IAuthentification
    {
        Task<(bool Success, string Token, DateTime Expiration, string Role, bool ActivateCompte)> Login(string email, string password);
        Task<(bool Success, string Message)> RegisterUserAsync(RegisterModel model);


    }
}


