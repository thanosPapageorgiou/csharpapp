using CSharpApp.Application.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void StoreToken(string accessToken, string refreshToken)
        {
            var session =  _httpContextAccessor.HttpContext.Session;
            session.Set("AccessToken", ConversionUtils.ConvertToByte(accessToken));
            session.Set("RefreshToken", ConversionUtils.ConvertToByte(refreshToken));
        }

        public string GetAccessToken()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            session.TryGetValue("AccessToken", out byte[] value);
            return ConversionUtils.ConvertToString(value);
        }

        public string GetRefreshToken()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.TryGetValue("RefreshToken", out byte[] value);
            return ConversionUtils.ConvertToString(value);
        }

        public void RemoveToken()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove("AccessToken");
            session.Remove("RefreshToken");
        }
    }
}
