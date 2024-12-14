namespace NotesBackend.Core.Models;

public record Note
{
    #region Properties

    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    #endregion
}