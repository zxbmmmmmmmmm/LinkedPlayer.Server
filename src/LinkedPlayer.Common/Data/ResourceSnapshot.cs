namespace LinkedPlayer.Common.Data;

public record ResourceSnapshot(
    Dictionary<string, object> Data,
    DateTimeOffset UpdatedAt);
