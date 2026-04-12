namespace HTP.Domain.Entities.RefreshTokens;

public sealed record RefreshToken
{
    public Guid Id { get; init; }
    public string Token { get; private set; } = default!;
    public DateTimeOffset ExpiredOnUtc { get; set; }
    public Guid UserId { get; set; }

    public static RefreshToken Create(string token, DateTimeOffset expiredOnUtc, Guid userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            ExpiredOnUtc = expiredOnUtc,
            UserId = userId
        };
    }

    public void UpdateRefreshToken(string newToken, DateTimeOffset expiredOnUtc)
    {
        Token = newToken;
        ExpiredOnUtc = expiredOnUtc;
    }
}

