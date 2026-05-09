using LinkedPlayer.Server.Models;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LinkedPlayer.Common.Data;

namespace LinkedPlayer.Server.Services;
public class RoomService
{
    private readonly Dictionary<string, Room> _rooms = [];

    private readonly Dictionary<string, RoomMember> _members = [];

    private readonly RoomIdGenerator _idGenerator = new();

    public CreateRoomResult CreateRoom(
        string name,
        string connectionId,
        string username, 
        Dictionary<string, object> resource)
    {
        var id = _idGenerator.Next().ToString();
        var creator = new RoomMember(connectionId, username, id);
        var room = new Room(id, name, creator, resource);
        _rooms[id] = room;
        _members[creator.ConnectionId] = creator;
        return new CreateRoomResult(id);
    }

    public MemberJoinedResult JoinRoom(string roomId, string connectionId, string username)
    {
        var room = _rooms.GetValueOrDefault(roomId);
        if(room is null)
            throw new ArgumentException($"Room with id {roomId} not found");
        var user = new RoomMember(connectionId, username, roomId);
        room.Members[user.ConnectionId] = user;
        _members[user.ConnectionId] = user;
        return new MemberJoinedResult(new ResourceSnapshot(room.Resource, room.ResourceUpdatedAt), new MemberJoinedEvent(username));
    }

    public MemberLeftResult LeaveRoom(string memberId)
    {
        var member = _members.GetValueOrDefault(memberId);
        if (member is null)
            throw new ArgumentException($"Member with id {memberId} not found");
        var roomId = member.RoomId;
        _members.Remove(memberId);
        var room = _rooms[roomId];
        room.Members.Remove(memberId);
        if(room.Members.Count == 0)
        {
            _rooms.Remove(roomId);
        }
        return new MemberLeftResult(member.RoomId,new MemberLeftEvent(member.Name));
    }

    public UpdateResourceResult UpdateResource(string memberId, Dictionary<string,object> values)
    {
        var member = _members.GetValueOrDefault(memberId);
        if (member is null)
            throw new ArgumentException($"Member with id {memberId} not found");
        var roomId = member.RoomId;
        var room = _rooms.GetValueOrDefault(roomId);
        if (room is null)
            throw new ArgumentException($"Room with id {roomId} not found");
        var updatedValues = new Dictionary<string, object>();
        foreach(var item in values)
        {
            ref var value = ref CollectionsMarshal.GetValueRefOrNullRef(room.Resource, item.Key);
            if (!Unsafe.IsNullRef(ref value) && !value.Equals(item.Value))
            {
                value = item.Value;
            }
            updatedValues[item.Key] = value;
        }
        return new UpdateResourceResult(member.RoomId, updatedValues);
    }

    private class RoomIdGenerator
    {
        private const int m = 900000;
        private const int a = 301;
        private const int c = 1;
        private int _count = 0;
        private int _current = (int)Random.Shared.NextInt64(m - 1);

        public int Next()
        {
            _current = (a * _current + c) % m;
            _count++;
            return _current;
        }
    }
}