using System.Text.Json.Serialization;

namespace NotesBackend.Api.Responses;

public class CreateNoteResponse
{
   #region Properties

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("body")]
    public required string Body { get; init; }

    [JsonPropertyName("createAt")]
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
        
    #endregion
}