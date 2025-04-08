using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CSharpApp.Application.Categories;

public class AuthService : IAuthService
{
    #region Properties
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CategoriesService> _logger;
    private readonly ITokenService _tokenService;
    #endregion

    #region Constructor
    public AuthService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CategoriesService> logger, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
        _tokenService = tokenService;
    }
    #endregion

    #region public Methods
    public async Task<string> GetToken()
    {
        string accessToken = string.Empty;

        string _accessTokenCache = _tokenService.GetAccessToken();

        if (!string.IsNullOrEmpty(_accessTokenCache))
        {
            bool tokenExpired = await IsTokenExpired(_accessTokenCache);
            if(!tokenExpired)
            {
                accessToken = _accessTokenCache;
            }
            else
            {
                string _refreshTokenCache = _tokenService.GetRefreshToken();
                if (!string.IsNullOrEmpty(_refreshTokenCache))
                {
                    AuthTokens tokens = await RefreshExistingToken(_refreshTokenCache!);
                    if (!string.IsNullOrEmpty(tokens.AccessToken))
                    {
                        accessToken = tokens.AccessToken;

                        _tokenService.StoreToken(tokens.AccessToken, tokens.RefreshToken!);
                    }
                    else
                    {
                        accessToken = await GenerateTokenAndStore();
                    }
                }
                else
                {
                    accessToken = await GenerateTokenAndStore();
                }
            }
        }
        else
        {
            accessToken = await GenerateTokenAndStore();
        }
        return accessToken;
    }
    #endregion

    #region Private Methods
    private async Task<AuthTokens> GenerateToken()
    {
        AuthTokens tokens = new();

        try
        {
            AuthUser user = new AuthUser
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
                tokens = res!;
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

        return tokens!;
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

        AuthUser user = new AuthUser
        {
            Email = _restApiSettings.Username,
            PassWord = _restApiSettings.Password
        };
        // This method execute a web API call to request a new accessToken using the refreshToken. 
        // Send the refreshToken form Session to the authorization server, returns a new accessToken.

        return tokens!;
    }
    private async Task<string> GenerateTokenAndStore()
    {
        string accessToken = string.Empty;
        AuthTokens tokens = await GenerateToken();
        if (!string.IsNullOrEmpty(tokens.AccessToken))
        {
            accessToken = tokens.AccessToken;

            _tokenService.StoreToken(tokens.AccessToken, tokens.RefreshToken!);
        }

        return accessToken;
    }
    #endregion
}