using System.Text;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace api.Data
{
    public class Token
    {
        public static string CreateToken(User user, string keyApp, IEnumerable<Permission> permissions)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("Name", user.Name),
                new Claim("RoleId", user.RoleId.ToString())
            };

            foreach (var item in permissions)
            {   
                claims.Add(new Claim(ClaimTypes.Role, item.Name));
            }


            var keyBytes = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(keyApp));
            var creds = new SigningCredentials(keyBytes, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public static string? ValidateToken(string token, string keyApp)
        {
            if (token == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(keyApp));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = keyBytes,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                return userId;
            }
            catch
            {
                return null;
            }
        }

    }
}