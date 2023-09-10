using Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(_ => new BlobService(builder.Configuration.GetConnectionString("BlobStorage")!));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("upload-file", async ([FromServices] BlobService blobService) =>
{
    var defaultFile = File.Open("./file.txt", FileMode.Open);
    await blobService.UploadFileAsync(defaultFile, "arquivo-exemplo.txt", "example");
    await defaultFile.DisposeAsync();
    return Results.Ok();
});
app.MapGet("partial-file", async ([FromQuery] long startAt, [FromQuery] long endAt, [FromServices] BlobService blobService) =>
{
    var result = await blobService.GetPartialFileAsync(startAt, endAt, "arquivo-exemplo.txt", "example");
    return Results.Ok(new { Content = result });
});
app.Run();