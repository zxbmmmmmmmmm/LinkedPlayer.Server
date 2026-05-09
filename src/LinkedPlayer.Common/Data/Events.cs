namespace LinkedPlayer.Common.Data;

public record MemberJoinedEvent(
    string DisplayName,
    string? UserId = null
);

public record MemberLeftEvent(
    string DisplayName,
    string? UserId = null
);

