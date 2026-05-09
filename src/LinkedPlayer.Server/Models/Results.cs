using LinkedPlayer.Common.Data;

namespace LinkedPlayer.Server.Models;

public record MemberLeftResult(
    string RoomId,
    MemberLeftEvent Event);

public record MemberJoinedResult(
    MemberJoinedEvent Event);

public record CreateRoomResult(
    string RoomId);

public record UpdateResourceResult(
    string RoomId,
    Dictionary<string, object> UpdatedValues);