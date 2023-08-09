using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Server.Authority
{
    public static class Authenticator
    {
        public static bool Authenticate(string clientId, string secret)
        {
            Application? app = AppRepository.GetApplicationByClientId(clientId);
            if (app == null)
            {
                return false;
            }
            else
            {
                return (app.ClientId == clientId) && (app.Secret == secret);
            }
        }

        public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
        {
            // Key
            byte[]? secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            Application? app = AppRepository.GetApplicationByClientId(clientId);

            // Claims
            List<Claim> claims = new List<Claim>()
            {
                new Claim("AppName", app?.ApplicationName??string.Empty)
                // ,
                // new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                // new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false")
            };

            // determine which claims are needed
            string[]? scopes = app?.Scopes?.Split(',');
            if (scopes is not null && scopes.Length > 0)
            {
                foreach (string scope in scopes)
                {
                    claims.Add(new Claim(scope.Trim().ToLower(), "true"));
                }
            }

            JwtSecurityToken jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                claims: claims,
                expires: expiresAt,
                notBefore: DateTime.UtcNow
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static IEnumerable<Claim>? VerifyToken(string token, string strSecretKey)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            if (token.ToLower().StartsWith("bearer"))
            {
                token = token.Substring(6).Trim();
            }

            byte[]? secretKey = Encoding.ASCII.GetBytes(strSecretKey);

            SecurityToken securityToken; 

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);

                if (securityToken is not null)
                {
                    JwtSecurityToken tokenObject = tokenHandler.ReadJwtToken(token)!;
                    // verification succeeded, get empty list if we have no claims
                    return tokenObject.Claims ?? (new List<Claim>()); 
                }
                else
                {
                    return null;
                }
            }
            catch (SecurityTokenException ex)
            {
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
