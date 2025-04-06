using CSharpApp.Application.Constants;
using CSharpApp.Application.Validation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CSharpApp.Application.Categories;

public class AuthService : IAuthService
{
    #region Properties
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CategoriesService> _logger;
    #endregion

    #region Constructor
    public AuthService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CategoriesService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }
    #endregion

    #region Private Methods
    public async Task<string> GetToken()
    {
        string accessToken = string.Empty;

        string _AccessTokenCache = "";
        string _RefreshTokenCache = "";

        if (!string.IsNullOrEmpty(_AccessTokenCache))
        {
            bool tokenExpired = await IsTokenExpired(_AccessTokenCache);
            if(!tokenExpired)
            {
                accessToken = _AccessTokenCache;
            }
            else
            {
                if(!string.IsNullOrEmpty(_RefreshTokenCache))
                {
                    AuthTokens Tokens = await RefreshExistingToken(_RefreshTokenCache!);
                    if (!string.IsNullOrEmpty(Tokens.AccessToken))
                    {
                        accessToken = Tokens.AccessToken;
                    }
                }
            }
        }
        else
        {
            AuthTokens Tokens = await GenerateToken();
            if (!string.IsNullOrEmpty(Tokens.AccessToken))
            {
                accessToken = Tokens.AccessToken;
            }
        }
        return accessToken;
    }
    #endregion

    #region Private Methods
    private async Task<AuthTokens> GenerateToken()
    {
        AuthTokens Tokens = new();

        try
        {
            User user = new User
            {
                Email = _restApiSettings.Username,
                PassWord = _restApiSettings.Password
            };

            var jsonContent = JsonSerializer.Serialize(user);
            var contentRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string authMethod = _restApiSettings.Auth ?? string.Empty;
            var response = await _httpClient.PostAsync(authMethod, contentRequest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<AuthTokens>(content);
                Tokens = res!;
            }
            else
            {
                ///Unauthorized
                _logger.LogError($"Failed to retrieve Tokens, statusCode: {response.StatusCode}");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Exception in AuthService.GenerateToken: {ex.Message}");
        }

        return Tokens!;
    }
    private async Task<bool> IsTokenExpired(string token)
    {
        // Create an instance of JwtSecurityTokenHandler
        var handler = new JwtSecurityTokenHandler();

        // Check if the token is readable by the handler
        if (handler.CanReadToken(token))
        {
            // Parse the token and get the JWT token object
            var jwtToken = handler.ReadJwtToken(token);

            // Get the 'exp' (expiration) claim from the payload
            var exp = jwtToken.Payload.Exp;

            // If 'exp' is null, we assume the token is expired
            if (exp.HasValue)
            {
                // Convert the Unix timestamp to a DateTimeOffset
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp.Value);

                // Check if the expiration time is in the past
                return expirationTime < DateTimeOffset.UtcNow;
            }
        }

        // Return true if the token is expired or cannot be read
        return true;
    }
    private async Task<AuthTokens> RefreshExistingToken(string refreshToken)
    {
        AuthTokens tokens = new();

        User user = new User
        {
            Email = _restApiSettings.Username,
            PassWord = _restApiSettings.Password
        };
        //here goes the web api call to generate new Token by refresh Token.

        return tokens!;
    }
    #endregion
}