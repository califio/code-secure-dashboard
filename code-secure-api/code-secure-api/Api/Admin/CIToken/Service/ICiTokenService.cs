using CodeSecure.Api.Admin.CIToken.Model;
using CodeSecure.Database.Entity;

namespace CodeSecure.Api.Admin.CIToken.Service;

public interface ICiTokenService
{
    public Task<List<CiTokens>> GetCiTokensAsync();
    public Task<bool> DeleteAsync(Guid id);
    public Task<CiTokens> CreateAsync(CreateCiTokenRequest request);
}