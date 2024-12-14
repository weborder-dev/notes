using System.Text.Json.Serialization;
using NotesBackend.Core.Models;

namespace NotesBackend.Api.Requests;

public class UpdateNoteRequest
{
    #region Properties

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("body")]
    public required string Body { get; init; }

    #endregion
}

public static class UpdateNoteRequestMappers
{
    #region Public Metods

    public static Note ToRequest(
        this UpdateNoteRequest req,
        string id)
    {
        return new Note
        {
            Id = id,
            Body = req.Body,
            Title = req.Title
        };
    }

    #endregion
}