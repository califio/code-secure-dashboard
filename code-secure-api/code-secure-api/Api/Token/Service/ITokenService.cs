using CodeSecure.Api.Token.Model;
using CodeSecure.Database.Entity;

namespace CodeSecure.Api.Token.Service;

public interface ITokenService
{
    public Task<List<CiTokens>> GetCiTokensAsync();
    public Task<bool> DeleteAsync(Guid id);
    public Task<CiTokens> CreateAsync(CreateTokenRequest request);
}