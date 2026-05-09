using LinkedPlayer.Server.Models;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LinkedPlayer.Common.Data;

namespace LinkedPlayer.Server.Services;
public class RoomService
{
    private readonly Dictionary<string, Room> _rooms = [];

    private readonly Dictionary<string, RoomMember> _members = [];

    public CreateRoomResult CreateRoom(
        string name,
        string connectionId,
        string username, 
        Dictionary<string, object> resource)
    {
        var creator = new RoomMember(connectionId, username, _rooms.Count.ToString());
        var room = new Room(_rooms.Count.ToString(), name, creator, resource);
        _rooms[room.Id] = room;
        _members[creator.ConnectionId] = creator;
        return new CreateRoomResult(room.Id);
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
}