namespace apief
{
    public interface IAuthRepository
    {
        public void CheckUser(UserForRegistrationDto userForRegistration);
        public string RegistrEndInsert(UserForRegistrationDto userForRegistration);
        public void CheckPassword(UserForLoginDto userForLogin);
        public string CheckEmail(UserForLoginDto userForLogin);
        
    }
}