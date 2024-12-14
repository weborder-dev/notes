using System.Text.Json.Serialization;

namespace NotesBackend.Api.Requests;

public class CreateNoteRequest
{
    #region Properties

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("body")]
    public required string Body { get; init; }
        
    #endregion
}
