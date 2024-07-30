using Cyclone.RepositoryService.Abstraction;
using Cyclone.Utilities;

namespace Cyclone.RepositoryService.Implementation
{
    public class TokenProvider : ITokenProvider
    {
        private readonly HttpContextAccessor _contextAccessor;
        public TokenProvider(HttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor; 
        }


        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(key: SD.Cookie!);
        }


        public string? GetToken()
        {
            var cookie = string.Empty;
            var hasCookie = _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.Cookie, out cookie);
            return hasCookie is true ? cookie : null;
        }


        public bool SetToken(string token)
        {
            if (token != null)
            {
                _contextAccessor.HttpContext?.Response.Cookies.Append(SD.Cookie, token);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
