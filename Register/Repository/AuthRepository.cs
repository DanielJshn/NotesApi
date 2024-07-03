using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace apief
{

    public class AuthRepository : IAuthRepository
    {
        private readonly DataContextEF _entityFramework;
        private readonly IConfiguration _config;
        
        private readonly IAuthHelp _authHelp;

        public AuthRepository(DataContextEF entityFramework, IAuthHelp authHelp, IConfiguration config)
        {
            _config = config;
            _entityFramework = entityFramework;
            _authHelp = authHelp;
            
        }




        public void CheckUser(UserForRegistrationDto userForRegistration)
        {
            using (var dbContext = new DataContextEF(_config))
            {
                var existingUser = dbContext.Accounts.FirstOrDefault(u => u.Email == userForRegistration.Email);

                if (existingUser != null)
                {
                    throw new Exception("User with this email already exists!");
                }
            }
        }




        public string RegistrEndInsert(UserForRegistrationDto userForRegistration)
        {
            byte[] passwordSalt;
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                passwordSalt = new byte[128 / 8];
                rng.GetBytes(passwordSalt);
            }

            byte[] passwordHash = _authHelp.GetPasswordHash(userForRegistration.Password, passwordSalt);
            string token = _authHelp.CreateToken(userForRegistration.Email);
            var tokenEntity = new Account
            {
                Email = userForRegistration.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                TokenValue = token
            };

            using (var dbContext = new DataContextEF(_config))
            {
                dbContext.Accounts.Add(tokenEntity);
                dbContext.SaveChanges();
            }

            return token;
        }




        public void CheckPassword(UserForLoginDto userForLogin)
        {
            var user = _entityFramework.Accounts.FirstOrDefault(u => u.Email == userForLogin.Email);
            if (user == null)
            {
                throw new Exception("Incorrect Email");
            }

            var userForConfirmation = _entityFramework.Accounts
                .Where(t => t.Email == userForLogin.Email)
                .Select(t => new UserForLoginConfirmationDto
                {
                    PasswordHash = t.PasswordHash,
                    PasswordSalt = t.PasswordSalt
                })
                .FirstOrDefault();

            byte[] inputPasswordHash = _authHelp.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt );
            if (!inputPasswordHash.SequenceEqual(userForConfirmation.PasswordHash))
            {
                throw new Exception("Incorrect Password");
            }
        }




        public string CheckEmail(UserForLoginDto userForLogin)
        {
            
            var token = _entityFramework.Accounts.FirstOrDefault(t => t.Email == userForLogin.Email);
            
            if (token == null)
            {
                throw new Exception("Неправильный адрес электронной почты!");
            }

            return token.TokenValue;
        }
    }
}
