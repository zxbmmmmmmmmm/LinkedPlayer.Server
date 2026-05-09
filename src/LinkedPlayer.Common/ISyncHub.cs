using System.Text.RegularExpressions;

namespace LinkedPlayer.Common;

public interface ISyncHub
{
    Task JoinRoom(string roomId, string userName);

    Task LeaveRoom();

    Task<string> CreateRoom(string name, string username, Dictionary<string, object> resource);

    Task UpdateResource(Dictionary<string, object> updatedValues);
}