using Cyclone.Services.AuthAPI.DTOs;
using Cyclone.Services.AuthAPI.Models;
using Cyclone.Services.AuthAPI.RepositoryServices.Abstraction;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cyclone.Services.AuthAPI.RepositoryServices.Implementation
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly JwtOptions _jwtOptions;

        public TokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
			_jwtOptions = jwtOptions.Value;
        }





        public string GenerateToken(UserDto user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			var claimList = new List<Claim>()
			{
				new(JwtRegisteredClaimNames.Sub, user.Id),
				new(JwtRegisteredClaimNames.Name, user.Name),
				new(JwtRegisteredClaimNames.Email, user.Email)
			};

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Issuer = _jwtOptions.Issuer,
				Audience = _jwtOptions.Audience,
				Subject = new ClaimsIdentity(claimList),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
