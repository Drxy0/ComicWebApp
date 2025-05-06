namespace ComicWebApp.Shared.DTOs;

public record RefreshTokenRequest(Guid UserId, string RefreshToken);
