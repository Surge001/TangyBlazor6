using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tangy.Common;
using Tangy.DataAccess;
using Tangy.Models;
using TangyApi.Helper;

namespace TangyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApiSettings apiSettings;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<ApiSettings> apiSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.roleManager = roleManager;
            this.apiSettings = apiSettings.Value;
        }

        [HttpPost(Name ="register")]
        public async Task<IActionResult> SignUp([FromBody] SignupRequestDto request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            ApplicationUser user = new()
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true,
                PhoneNumber = request.PhoneNumber,
                Name = request.Name
            };
            IdentityResult? result = await this._userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new SignupResponseDto()
                {
                    IsRegistrationSuccessful = false,
                    Errors = result.Errors.Select(i => i.Description).ToList()
                });
            }

            var roleResult = await this._userManager.AddToRoleAsync(user, SD.Role_Customer);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new SignupResponseDto()
                {
                    IsRegistrationSuccessful = false,
                    Errors = roleResult.Errors.Select(i => i.Description).ToList()
                });
            }
            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await this._signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if(user == null)
                {
                    return Unauthorized(new SignInResponseDto()
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid Authentication"
                    });
                }
                SigningCredentials signinCredientials = this.GetSignInCredentials();
                List<Claim> claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                    issuer: this.apiSettings.ValidIssuer,
                    audience: this.apiSettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signinCredientials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                int userId = 0;
                return Ok(new SignInResponseDto()
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    UserDto = new UserDto()
                    {
                        Name = user.Name,
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    }

                });
            }
            else
            {

                return Unauthorized(new SignInResponseDto()
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }
        }


        private SigningCredentials GetSignInCredentials()
        {
            string secretKey = this.apiSettings.SecretKey;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id)
            };

            var roles = await this._userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
