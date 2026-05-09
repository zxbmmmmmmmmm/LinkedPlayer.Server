namespace LinkedPlayer.Server.Models;

public class Room
{
    public string Id { get; set; }

    public string Name { get; set; }

    public Dictionary<string, RoomMember> Members { get; set; }

    public Dictionary<string, object> Resource { get; set; }

    public DateTimeOffset ResourceUpdatedAt { get; set; }

    public DateTimeOffset CreatedAt { get; init; }

    public RoomMember Owner { get; set;  }

    public Room(string id, string name, RoomMember creator, Dictionary<string, object> resource)
    {
        Id = id;
        Name = name;
        Members = new Dictionary<string, RoomMember> { { creator.ConnectionId, creator } };
        Resource = resource;
        CreatedAt = DateTimeOffset.Now;
        ResourceUpdatedAt = DateTimeOffset.Now;
        Owner = creator;
    }
}
