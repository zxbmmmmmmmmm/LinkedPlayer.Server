using System.Net.WebSockets;

namespace LinkedPlayer.Server.Models;

public record RoomMember
{
    public string ConnectionId { get; set; }

    public string Name { get; set; }

    public string RoomId { get; set; }

    public DateTimeOffset JoinedAt { get; set; }

    public RoomMember(string id, string name, string roomId)
    {
        ConnectionId = id;
        Name = name;
        RoomId = roomId;
        JoinedAt = DateTimeOffset.UtcNow;
    }
}
