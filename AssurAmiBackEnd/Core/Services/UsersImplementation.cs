using AssurAmiBackEnd.Core.Entity.Authentification;
using AssurAmiBackEnd.Infrastructure.Persistance.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssurAmiBackEnd.Core.Services
{
    public class UsersImplementation : IUsers
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersImplementation(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message)> disabledAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "Utilisateur introuvable");
            }

            if (!user.ActivateCompte)
            {
                return (false, "Le compte est déjà désactivé");
            }

            user.ActivateCompte = false;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return (true, "Le compte a été désactivé avec succès");
            }
            else
            {
                return (false, "Erreur lors de désactivation du compte");
            }
        }

        public async Task<(bool Success, string Message)> enableAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, "Utilisateur introuvable");
            }

            if (user.ActivateCompte)
            {
                return (false, "Le compte est déjà activé");
            }

            user.ActivateCompte = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return (true, "Le compte a été activé avec succès");
            }
            else
            {
                return (false, "Erreur lors de l'activation du compte");
            }
        }

        public async Task<(IEnumerable<ApplicationUser> applicationUsers, int totalCount)> GetAllUsersAsync()
        {
            var totalUsers = await _context.ApplicationUsers.CountAsync();
            var users = await _context.ApplicationUsers.ToListAsync();
            return (users, totalUsers);
        }

        public async Task<(bool Success, string Message)> updatePassword(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }
    }
}
