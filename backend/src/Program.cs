using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NATS.Client.Core;
using NATS.Client.Hosting;
using NATS.Client.Serializers.Json;
using NotesBackend.Api.Requests;
using NotesBackend.Api.Responses;
using NotesBackend.Core.Abstractions;
using NotesBackend.Core.Data.Stores;
using NotesBackend.Core.Models;
using NotesBackend.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<INotesService, NotesService>();
builder.Services.AddTransient<INotesStore, NATSNotesStore>();

// builder.Services.AddNats(configureConnection: c => c = conn);
builder.Services.AddNats(1, opc => NatsOpts.Default with
{
    Url = Environment.GetEnvironmentVariable("EVS_CONNECTION_URL")
        ?? "nats://localhost:4222",
    
    SerializerRegistry = NatsJsonSerializerRegistry.Default
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetEntryAssembly());

builder.Services.AddCors(opc => opc.AddPolicy("ncors",
    builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

app.UseCors("ncors");

app.MapPost("/notes", async (
    INotesService svr,
    [FromBody] CreateNoteRequest request) =>
{
    var note = new Note
    {
        Id = Guid.NewGuid().ToString("N"),
        Title = request.Title,
        Body = request.Body
    };

    var res = await svr.CreateNoteAsync(note);

    if (res.Code == 200 && res.Data is not null)
    {
        var response = new CreateNoteResponse
        {
            Id = res.Data.Id,
            Title = res.Data.Title,
            Body = res.Data.Body,
            CreatedAt = res.Data.CreatedAt
        };

        return Results.Ok(response);
    }

    return Results.BadRequest(res.Errors);
});

app.MapGet("/notes/{id}", async (
    INotesService svr,
    [FromRoute] string id) =>
{
    var result = await svr.GetNoteByIdAsync(id);

    if (result.Data is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(result.Data);
});

app.MapGet("/notes/all", async (
    INotesService svr) =>
{
    var result = await svr.GetAllNotesAsync();

    if (result.Data is null)
    {
        return Results.NoContent();
    }

    return Results.Ok(result.Data);
});

app.MapDelete("/notes/{id}", async (
    INotesService svr,
    [FromRoute] string id) =>
{
    await svr.DeleteNoteAsync(id);
    return Results.Ok();
});

app.MapPatch("/notes/{id}", async (
    INotesService svr,
    [FromRoute] string id,
    [FromBody] UpdateNoteRequest request) =>
    {
        var res = await svr.UpdateNoteAsync(
            id,
            request.ToRequest(id));

        if (res.Data is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(res.Data);
    });

app.Run();
