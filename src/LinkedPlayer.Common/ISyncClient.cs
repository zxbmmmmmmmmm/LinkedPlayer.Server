using LinkedPlayer.Common.Data;

namespace LinkedPlayer.Common;

public interface ISyncClient
{
    Task ResourceUpdated(Dictionary<string, object> updatedValues);

    Task MemberJoined(MemberJoinedEvent data);

    Task MemberLeft(MemberLeftEvent data);
}