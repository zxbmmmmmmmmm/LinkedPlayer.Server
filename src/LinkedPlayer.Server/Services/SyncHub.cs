using LinkedPlayer.Server.Models;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using LinkedPlayer.Common;

namespace LinkedPlayer.Server.Services;

[RequiresDynamicCode("")]
public class SyncHub : Hub<ISyncClient> , ISyncHub
{
    public RoomService _roomService;

    public SyncHub(RoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task JoinRoom(string roomId, string userName)
    {
        var connectionId = Context.ConnectionId;
        var result = _roomService.JoinRoom(roomId, connectionId, userName);
        await Groups.AddToGroupAsync(connectionId, roomId);
        await Clients.OthersInGroup(roomId).MemberJoined(result.Event);
    }

    public async Task LeaveRoom()
    {
        var connectionId = Context.ConnectionId;
        var result = _roomService.LeaveRoom(connectionId);
        await Groups.RemoveFromGroupAsync(connectionId, result.RoomId);
        await Clients.Group(result.RoomId).MemberLeft(result.Event);
    }

    public async Task<string> CreateRoom(string name, string username, Dictionary<string, object> resource)
    {
        var connectionId = Context.ConnectionId;
        var result = _roomService.CreateRoom(name, Context.ConnectionId, username, resource);
        await Groups.AddToGroupAsync(connectionId, result.RoomId);
        return result.RoomId;
    }

    public async Task UpdateResource(Dictionary<string, object> updatedValues)
    {
        var result = _roomService.UpdateResource(Context.ConnectionId, updatedValues);
        await Clients.OthersInGroup(result.RoomId).ResourceUpdated(result.UpdatedValues);
    }
}

