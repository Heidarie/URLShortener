using URLShortener.Core;
using URLShortener.Core.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
    builder.Configuration.AddUserSecrets<Program>();
}


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCore();
builder.Services.MigrateDatabase();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/{id}", async (string id, IUrlShortenerService service) =>
    {
        var result = await service.GetById(id);
        return result is not null ? Results.Redirect(result) : Results.NotFound();
    })
.WithName("GetShortUrlById")
.WithOpenApi();

app.Run();