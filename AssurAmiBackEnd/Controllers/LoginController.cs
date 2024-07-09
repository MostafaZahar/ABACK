using AssurAmiBackEnd.Core.Entity;
using AssurAmiBackEnd.Core.Entity.Authentification;
using AssurAmiBackEnd.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssurAmiBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase

    {
        

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAuthentification _authentification;

        public LoginController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
             IAuthentification authentification)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _authentification = authentification;  
        }


        //Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("email or password inccorect please retry ");
                return BadRequest(ModelState);
            }

            var result = await _authentification.Login(model.Email, model.Password);
            if (result.Success)
            {
                return Ok(new
                {
                    token = result.Token,
                    expiration = result.Expiration,
                    role = result.Role,
                    activateCompte = result.ActivateCompte,
                });
            }
            else
            {
                return Unauthorized(new
                {
                    error = "Invalid login attempt",
                    activateCompte = result.Success ? true : false  // Utilisation de la valeur réelle en cas d'échec
                });
            }
        }


        //Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authentification.RegisterUserAsync(model);
            if (result.Success)
            {
                return Ok(new Response { Status = "Success", Message = result.Message });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result.Message });
        }







        //RegisterAdmin
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRole.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            if (!await roleManager.RoleExistsAsync(UserRole.User))
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));

            if (await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRole.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        
    }
    


}

    //[HttpPost]
    //public IActionResult Post([FromBody] LoginRequest loginRequest)
    //{
    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //    var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
    //      _configuration["Jwt:Issuer"],
    //      null,
    //      expires: DateTime.Now.AddMinutes(120),
    //      signingCredentials: credentials);

    //    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
    //    Console.WriteLine(token);

    //    return Ok(token);
    //}}

