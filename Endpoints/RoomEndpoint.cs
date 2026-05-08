namespace LinkedPlayer.Server.Endpoints;

public static class RoomEndpoint
{
    
    extension(WebApplication builder)
    {
        public void MapRoomEndpoints()
        {
            builder.MapGet("/room", () => "Hello Room!");
        }
    }
}
