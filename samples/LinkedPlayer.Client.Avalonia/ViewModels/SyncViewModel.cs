using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LinkedPlayer.Common;
using LinkedPlayer.Common.Data;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypedSignalR.Client;

namespace LinkedPlayer.Client.Avalonia.ViewModels;

public partial class SyncViewModel : ObservableObject, ISyncClient
{
    private ISyncHub? _hubProxy;
    private bool _syncing;

    [ObservableProperty]
    public partial string Message { get; set; } = "";

    [ObservableProperty]
    public partial string Username { get; set; } = "";

    [ObservableProperty]
    public partial string RoomId { get; set; } = "";

    [RelayCommand]
    public async Task Connect()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5180/sync")
            .Build();
        connection.Register<ISyncClient>(this);
        await connection.StartAsync();
        _hubProxy = connection.CreateHubProxy<ISyncHub>();
    }

    [RelayCommand]
    public async Task CreateRoom()
    {
        await Connect();
        if (_hubProxy is null) return;
        var id = await _hubProxy.CreateRoom("Test Room", Username, new Dictionary<string, object> { { nameof(Message), Message } });
        RoomId = id;
    }


    [RelayCommand]
    public async Task JoinRoom()
    {
        await Connect();
        if (_hubProxy is null) return;
        await _hubProxy.JoinRoom(RoomId,Username);
    }

    public Task MemberJoined(MemberJoinedEvent data)
    {
        return Task.CompletedTask;
    }

    public Task MemberLeft(MemberLeftEvent data)
    {
        return Task.CompletedTask;
    }

    public Task ResourceUpdated(Dictionary<string, object> updatedValues)
    {
        _syncing = true;
        foreach (var kvp in updatedValues) 
        { 
            if(kvp.Key == nameof(Message))
            {
                Message = kvp.Value.ToString() ?? "";
            }
        }
        _syncing = false;
        return Task.CompletedTask;
    }



    partial void OnMessageChanged(string value)
    {
        if (_syncing)
            return;
        _hubProxy?.UpdateResource(new Dictionary<string, object> { { nameof(Message), value } });
    }
}