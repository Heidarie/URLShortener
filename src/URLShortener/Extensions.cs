using URLShortener.Abstractions.Services.Interfaces;
using URLShortener.Core.Exceptions;
using URLShortener.Shared.DTOS.Input;

namespace URLShortener;

public static class Extensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/{id}", async (string id, IUrlShortenerService service) =>
            {
                try
                {
                    var result = await service.GetById(id);
                    return result is not null ? Results.Ok(result) : Results.NotFound();
                }
                catch (UrlRetrievalFailedException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500);
                }
            })
            .WithName("GetShortUrlById")
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .WithOpenApi();

        app.MapPut("", async (CreateShortenedUrlDto dto, IUrlShortenerService service, HttpContext httpContext) =>
            {
                if (!string.IsNullOrEmpty(dto.Id))
                {
                    var targetUrl =
                        $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}/{dto.Id}";
                    if (dto.OriginalUrl == targetUrl)
                        return Results.BadRequest("Original URL cannot be the same as the shortened URL");
                }

                try
                {
                    var result = await service.Create(dto);
                    return Results.Created($"/{result.Id}", result);
                }
                catch (IdPatternNotMatchedException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (IdAlreadyExistsException ex)
                {
                    return Results.Conflict(ex.Message);
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500);
                }
            })
            .Produces(400)
            .Produces(409)
            .Produces(500)
            .WithName("CreateShortUrl")
            .WithOpenApi();

        app.MapDelete("/{id}", async (string id, IUrlShortenerService service) =>
            {
                try
                {
                    await service.Delete(id);
                    return Results.NoContent();
                }
                catch (EntityNotFoundException)
                {
                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.Problem(statusCode: 500);
                }
            })
            .WithName("DeleteShortUrl")
            .Produces(404)
            .Produces(500)
            .WithOpenApi();
    }
}