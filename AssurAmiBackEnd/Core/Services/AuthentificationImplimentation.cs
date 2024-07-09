using AssurAmiBackEnd.Core.Entity.Authentification;
using AssurAmiBackEnd.Infrastructure.Persistance.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AssurAmiBackEnd.Core.Services
{
    public class AuthentificationImplimentation : IAuthentification
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;


        public AuthentificationImplimentation(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }



        //public async Task<(bool Success, string Token, DateTime Expiration)> Login(string email, string password)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Email, user.Email),
        //        //new Claim(ClaimTypes.Role,user),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id) // Add UserId claim
        //    };

        //        foreach (var userRole in userRoles)
        //        {
        //            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //        }

        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["JWT:Issuer"],
        //            audience: _configuration["JWT:Audience"],
        //            expires: DateTime.Now.AddHours(3),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );



        //        return (true, new JwtSecurityTokenHandler().WriteToken(token) ,  token.ValidTo);
        //    }

        //    return (false, null, DateTime.MinValue);


        //}

        public async Task<(bool Success, string Token, DateTime Expiration, string Role, bool ActivateCompte)> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null  && await _userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Count == 0)
                {
                    return (false, null, DateTime.MinValue, null, false);
                }

                var userRole = userRoles[0]; // Assuming each user has only one role

                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, userRole)
        };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddDays(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return (true, new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo, userRole, user.ActivateCompte);
            }

            return (false, null, DateTime.MinValue, null, false);
        }




        public async Task<(bool Success, string Message)> RegisterUserAsync(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
                return (false, "User already exists!");

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                ActivateCompte = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return (false, "User creation failed! Please check user details and try again.");

            // Check if the role exists, if not create it
            if (!await _roleManager.RoleExistsAsync(model.Role.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Role.ToString()));
            }

            // Assign the role to the user
            await _userManager.AddToRoleAsync(user, model.Role.ToString());

            return (true, "User created successfully!");
        }
    }
}
