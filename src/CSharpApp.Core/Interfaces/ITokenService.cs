namespace CSharpApp.Core.Interfaces;

public interface ITokenService
{
    void StoreToken(string accessToken, string refreshToken);
    string GetAccessToken();
    string GetRefreshToken();
    void RemoveToken();
}