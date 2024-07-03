namespace apief
{
    public interface IAuthHelp
    {
        public byte[] GetPasswordHash(string password, byte[] passwordSalt);
        public string CreateToken(string userEmail);
        public string? GetUserEmailFromDatabase(int userId, IConfiguration config);
        public string GenerateNewToken(int userId);
        public int GetUserIdFromToken(string? accessToken);
        public bool UpdateTokenValueInDatabase(int userId, string newToken);
    }

}