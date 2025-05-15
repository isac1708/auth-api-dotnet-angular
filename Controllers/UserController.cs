using AuthApi.Data;
using AuthApi.DTO;
using AuthApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AuthApi.Services;

namespace AuthApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new(); // cria um PasswordHasher para o User

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        /// Método para registrar um novo usuário
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Username == dto.Username))
            {
                return BadRequest("Usuário já existe.");
            }

            var user = new User
            {
                Username = dto.Username
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);// cria o hash da senha

            // Salva no banco
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuário registrado com sucesso.");
        }

        /// Método para obter o usuário autenticado
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto dto, [FromServices] JwtService jwtService)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized("Usuário inválido.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Senha incorreta.");
            }

            var token = jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}

