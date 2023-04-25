using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StoreModels.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        JWTResult JWTResult;

        public LoginController()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            JWTResult = config.GetRequiredSection("JWT").Get<JWTResult>();
        }

        [HttpPost("Login")]
        public Token Login(Token token)
        {
            Token tokenResult = new Token();

            if (token.TokenContent == "asdfghjkl123456789")
            {
                string appName = "webAPI";
                tokenResult.ExpTime = DateTime.Now.AddMinutes(15);
                tokenResult.TokenContent = CustomTokenJWT(appName, tokenResult.ExpTime);
            }

            return tokenResult;
        }

        private string CustomTokenJWT(string ApplicationName, DateTime token_expiration)

        {

            var _symmetricSecurityKey = new SymmetricSecurityKey(

                    Encoding.UTF8.GetBytes(JWTResult.SecretKey)

                );

            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );

            var _Header = new JwtHeader(_signingCredentials);

            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, ApplicationName),
                new Claim("UserId", "id"),
                new Claim("RoleId", "id"),
            };

            var _Payload = new JwtPayload(
                    issuer: JWTResult.Issuer,
                    audience: JWTResult.Audience,
                    claims: _Claims,
                    notBefore: DateTime.Now,
                    expires: token_expiration
                );

            var _Token = new JwtSecurityToken(

                    _Header,

                    _Payload

                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);

        }
    }
}
