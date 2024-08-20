using Microsoft.AspNetCore.Mvc;

namespace apief
{
    public class TokenOpperation : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenOpperation(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        protected virtual int GetUserId()
        {
            string? accessToken = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];

            if (accessToken != null && accessToken.StartsWith("Bearer "))
            {
                accessToken = accessToken.Substring("Bearer ".Length).Trim();
            }
            else
            {
                throw new Exception("Authorization header is missing or invalid.");
            }

            int userId = 0;

            using (var dbContext = new DataContextEF(_config))
            {
                var token = dbContext.Accounts.FirstOrDefault(t => t.TokenValue == accessToken);
                if (token != null)
                {
                    userId = token.Id;
                    return userId;
                }
            }

            throw new Exception("Token not found in database.");
        }

        public int GetUserIdFromToken()
        {
            return GetUserId();
        }
    }
}