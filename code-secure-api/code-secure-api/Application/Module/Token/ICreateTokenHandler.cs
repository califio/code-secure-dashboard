using System.ComponentModel.DataAnnotations;
using CodeSecure.Core.Entity;
using FluentResults;

namespace CodeSecure.Application.Module.Token;

public record CreateTokenRequest
{
    [Required] 
    public required string Name { get; set; }
}
public interface ICreateTokenHandler : IHandler<CreateTokenRequest, CiTokens>;
public class CreateTokenHandler(AppDbContext context): ICreateTokenHandler
{
    public async Task<Result<CiTokens>> HandleAsync(CreateTokenRequest request)
    {
        var tokenName = request.Name.Trim();
        if (context.CiTokens.Any(record => record.Name == tokenName))
        {
            return Result.Fail("Duplicate token name");
        }

        var tokenValue = $"{Guid.NewGuid()}{Guid.NewGuid()}".Replace("-", "");
        var token = new CiTokens
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Name = tokenName,
            Value = tokenValue,
            ExpiredAt = null
        };
        context.CiTokens.Add(token);
        await context.SaveChangesAsync();
        return token;
    }
}