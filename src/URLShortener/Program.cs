using URLShortener.Core;

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

app.Run();