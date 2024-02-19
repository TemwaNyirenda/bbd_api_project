using UkukhulaAPI.Data.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using UkukhulaAPI.Data.Services;
using UkukhulaAPI.Controllers.Request;

namespace JWTLoginAuthenticationAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsersService _userService;
        private readonly IConfiguration _config;
        public LoginController(IConfiguration config, UsersService usersService)
        {
            _userService = usersService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginVm userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("user not found");
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody]UserRegistrationRequest userRegistrationRequest)
        {
           if(_userService.AddUser(userRegistrationRequest.User,userRegistrationRequest.userContact))
           {
            return Ok();

           }
            return BadRequest("unable to create user");
        }

        string GenerateJwtKey()
        {
            // Define the length of the key in bytes (32 bytes for HMACSHA256)
            int keyLengthBytes = 32; // 256 bits

            // Generate a random key with the specified length
            byte[] keyBytes = new byte[keyLengthBytes];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }

            // Convert the byte array to a base64-encoded string
            return Convert.ToBase64String(keyBytes);
        }
        // To generate token
        private string GenerateToken(LoginVm user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GenerateJwtKey()));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private LoginVm Authenticate(LoginVm userLogin)
        {
            var currentUsern = _userService.findUser(userLogin.Username);
            var userRole = _userService.FindUserRole(currentUsern);
            var password = "12345";
            // var currentUser = UserConstants.Users.FirstOrDefault(x => x.Username.ToLower() ==
            //     userLogin.Username.ToLower() && x.Password == userLogin.Password);
            if (currentUsern != null)
            {
                currentUsern.Password = password;
                currentUsern.Role = userRole;
                return currentUsern;
            }
            return null;
        }
    }
}