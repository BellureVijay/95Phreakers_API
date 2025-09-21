using _95PhrEAKer.Domain.AuthModal;
using _95PhrEAKer.Persistence.context;
using _95PhrEAKer.Persistence.DbModals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _95PhrEAKer_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public IConfiguration Configuration { get; }
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;

            Configuration = configuration;
        }

        [HttpPost]
        [Route("/auth/login")]
        public IActionResult Authenticate([FromBody] Credentials credential)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == credential.Email && u.Password == credential.Password); 

            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim("name", user.UserName),
            new Claim("Email", user.Email),
            new Claim("mobileNumber",user.MobileNumber),

        };

                var expiryTime = DateTime.Now.AddMinutes(60);
                return Ok(new
                {
                    access_Token = CreateToken(claims, expiryTime),
                    expiresAt = expiryTime,
                });
            }

            ModelState.AddModelError("UnAuthorized", "You are not authorized to access the endpoint");
            return Unauthorized(ModelState);
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expireAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("secreteKey") ?? "");
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                   )
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        [HttpPost]
        [Route("/auth/register")]
        public IActionResult Register([FromBody] UserModal user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                if (_context.Users.Any(x => x.Email == user.Email))
                {
                    if (user.isGoogleUser)
                    {
                        var password = _context.Users.FirstOrDefault(u =>
               u.Email == user.Email)?.Password;
                        return Authenticate(new Credentials { Email = user.Email, Password = password ?? "" });
                    }
                    return BadRequest(new { error = "Email address already exists." });
                }

                if (_context.Users.Any(x => x.UserName == user.UserName))
                {
                    return BadRequest(new { error = "Username already exists." });
                }


                var newUser = new Users
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    MobileNumber = user.MobileNumber,
                    Password = user.Password,
                    CreatedDate=DateTime.UtcNow
                };
                _context.Users.Add(newUser);
                
                _context.SaveChanges();
                if (user.isGoogleUser)
                {
                    return Authenticate(new Credentials { Email = user.Email, Password = user.Password ?? "" });
                }
                else
                {
                    return Ok($"{user.UserName} registered successfully.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "An error occurred while saving data.");
            }
        }


        public class UpdateProfileDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPut]
        [Route("/auth/updateProfile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == model.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.Password = model.Password;
                user.LastUpdated = DateTime.UtcNow;
                _context.SaveChanges();

                return Ok("Password changed successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while saving data.");
            }
        }

    }

}
