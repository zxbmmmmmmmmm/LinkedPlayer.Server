using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using LinkedPlayer.Common.Data;
using LinkedPlayer.Server.Services;

var builder = WebApplication.CreateSlimBuilder(args);

//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
//});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddSingleton<RoomService>();

var app = builder.Build();

app.MapHub<SyncHub>("/sync");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/rooms", () => {

});

app.Run();


[JsonSerializable(typeof(MemberJoinedEvent))]
[JsonSerializable(typeof(MemberLeftEvent))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(DateOnly))]
[JsonSerializable(typeof(TimeOnly))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(Dictionary<string, object>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
