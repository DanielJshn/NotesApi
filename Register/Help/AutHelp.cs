using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace apief
{
    public class AuthHelp : IAuthHelp
    {

        private readonly IConfiguration _config;
        DataContextEF _entity;
        public AuthHelp(IConfiguration config, DataContextEF entity)
        {
            _config = config;
            _entity = entity;
        }



        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string? passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000000,
                numBytesRequested: 256 / 8
            );
        }




        public string CreateToken(string userEmail)
        {
            Claim[] claims = new Claim[]
            {
              new Claim("Email", userEmail)
            };

            string? TokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKeyString != null ? TokenKeyString : ""));
            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddMonths(1)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }




        public string? GetUserEmailFromDatabase(int userId, IConfiguration config)
        {
            using (var dbContext = new DataContextEF(config))
            {
                var userEmail = dbContext.Accounts
                    .Where(t => t.Id == userId)
                    .Select(t => t.Email)
                    .FirstOrDefault();

                return userEmail;
            }
        }









        public int GetUserIdFromToken(string? accessToken)
        {
            int userId = 0;
            if (accessToken != null && accessToken.StartsWith("Bearer "))
            {
                accessToken = accessToken.Substring("Bearer ".Length);
            }

            accessToken = accessToken?.Trim();

            using (var dbContext = new DataContextEF(_config))
            {
                var token = dbContext.Accounts.FirstOrDefault(t => t.TokenValue == accessToken);
                if (token != null)
                {
                    userId = token.Id;
                }
            }

            return userId;
        }




        public bool UpdateTokenValueInDatabase(int userId, string newToken)
        {
            try
            {
                string? connectionString = _config.GetConnectionString("DefaultConnection");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sqlUpdate = "UPDATE Account SET TokenValue = @NewToken WHERE Id = @UserId";
                    SqlCommand updateCommand = new SqlCommand(sqlUpdate, conn);
                    updateCommand.Parameters.AddWithValue("@NewToken", newToken);
                    updateCommand.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating token in database: {ex.Message}");
                return false;
            }
        }




        public string GenerateNewToken(int userId)
        {
            var user = _entity.Accounts.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("UserId is not found");
            }

            string userEmail = user.Email;
            Claim[] claims = new Claim[]
            {
              new Claim("Email", userEmail)
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey")?.Value;
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString ?? ""));
            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()

            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddMonths(1)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken newtoken = handler.CreateToken(descriptor);

            return handler.WriteToken(newtoken);
        }

        
    }

}