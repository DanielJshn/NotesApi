using Microsoft.IdentityModel.Tokens;

namespace apief
{


    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public void CheckUser(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Email.IsNullOrEmpty())
            {
                throw new Exception("Email is empty!");
            };
            if (userForRegistration.Password.IsNullOrEmpty())
            {
                throw new Exception("Password is empty!");
            };
            _authRepository.CheckUser(userForRegistration);
        }
        public string RegistrEndInsert(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Email.IsNullOrEmpty())
            {
                throw new Exception("Email is empty!");
            };
            if (userForRegistration.Password.IsNullOrEmpty())
            {
                throw new Exception("Password is empty!");
            };
            return _authRepository.RegistrEndInsert(userForRegistration);
        }
        public void CheckPassword(UserForLoginDto userForLogin)
        {
            if (userForLogin.Email.IsNullOrEmpty())
            {
                throw new Exception("Email is empty!");
            };
            if (userForLogin.Password.IsNullOrEmpty())
            {
                throw new Exception("Password is empty!");
            };
            _authRepository.CheckPassword(userForLogin);
        }
        public string CheckEmail(UserForLoginDto userForLogin)
        {
            if (userForLogin.Email.IsNullOrEmpty())
            {
                throw new Exception("Email is empty!");
            };
            if (userForLogin.Password.IsNullOrEmpty())
            {
                throw new Exception("Password is empty!");
            };
            return _authRepository.CheckEmail(userForLogin);
        }


    }
}