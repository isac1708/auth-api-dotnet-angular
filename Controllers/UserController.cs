using AuthApi.Data;
using AuthApi.DTO;
using AuthApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            // Salva no banco
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuário registrado com sucesso.");
        }
    }
}

