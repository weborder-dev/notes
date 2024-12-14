using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.KeyValueStore;
using NotesBackend.Core.Abstractions;
using NotesBackend.Core.Models;

namespace NotesBackend.Core.Stores;

public class NATSNotesStore : INotesStore
{
    #region Fields

    private readonly INatsConnection _nats;
    private readonly ILogger _logger;

    #endregion

    #region Constructors

    public NATSNotesStore(
        INatsConnection nats,
        ILogger<NATSNotesStore> logger)
    {
        _nats = nats;
        _logger = logger;
    }

    #endregion

    #region Public Methods

    public async Task<Note> CreateNoteAsync(Note note)
    {
        var kv = await GetOrCreateStoreAsync();

        await kv.PutAsync(note.Id, note);

        return note;
    }

    public async Task<bool> DeleteNote(string id)
    {
        try
        {
            var kv = await GetOrCreateStoreAsync();
            await kv.PurgeAsync(id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<Note>> GetAllNotesAsync()
    {
        var kv = await GetOrCreateStoreAsync();
        var notes = new List<Note>();

        var keys = kv.GetKeysAsync();
        await foreach (var key in keys)
        {
            var entry = await kv.GetEntryAsync<Note>(key);
            if (entry.Value is not null)
            {
                notes.Add(entry.Value);
            }
        }

        return notes;
    }

    public async Task<Note?> GetNoteByIdAsync(string id)
    {
        try
        {
            var kv = await GetOrCreateStoreAsync();
            var entry = await kv.GetEntryAsync<Note>(id);
            return entry.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Note?> UpdateNoteAsync(Note note)
    {
        try
        {
            var kv = await GetOrCreateStoreAsync();
            await kv.PutAsync(note.Id, note);

            return note;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region Private Methods

    private async Task<INatsKVStore> GetOrCreateStoreAsync(CancellationToken cancellationToken = default)
    {
        var jsCtx = new NatsJSContext(_nats);
        var kvCtx = new NatsKVContext(jsCtx);

        var store = await kvCtx.CreateStoreAsync(new NatsKVConfig("notes")
        {
            Description = "Notes",
            Compression = false,
            NumberOfReplicas = 1
        }, cancellationToken);

        return store;
    }

    #endregion
}