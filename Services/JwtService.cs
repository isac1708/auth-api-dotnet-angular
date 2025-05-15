using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var  key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));// esta linha faz a codificação da chave
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // esta linha cria as credenciais de assinatura

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username), // esta linha cria a reivindicação do nome de usuário
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // esta linha cria uma nova reivindicação de ID
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // esta linha define o emissor do token
                audience: _configuration["Jwt:Audience"], // esta linha define o público do token
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // esta linha define a expiração do token
                signingCredentials: creds);// esta linha define as credenciais de assinatura

            return new JwtSecurityTokenHandler().WriteToken(token); // esta linha cria o token JWT
        }



    }
}
